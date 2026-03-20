#include "runtime.h"
#include "logger.h"
#include "payload.h"

#include <TlHelp32.h>

namespace mrk {

	DWORD getProcessPID(const std::string& processName) {
		LOG("Getting PID for process name: %s", processName.c_str());

		HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
		if (hSnapshot == INVALID_HANDLE_VALUE) {
			LOG("Failed to create process snapshot. Error: %lu", GetLastError());
			return 0;
		}

		PROCESSENTRY32 procEntry;
		procEntry.dwSize = sizeof(PROCESSENTRY32);
		if (!Process32First(hSnapshot, &procEntry)) {
			LOG("Process32First failed. Error: %lu", GetLastError());
			CloseHandle(hSnapshot);
			return 0;
		}

		do {
			if (processName == procEntry.szExeFile) {
				DWORD pid = procEntry.th32ProcessID;
				VLOG("Found process '%s' with PID: %lu", processName.c_str(), pid);
				CloseHandle(hSnapshot);
				return pid;
			}
		} while (Process32Next(hSnapshot, &procEntry));

		VLOG("Process '%s' not found.", processName.c_str());
		CloseHandle(hSnapshot);
		return 0;
	}

	bool inject(DWORD pid) {
		LOG("Injecting into process with PID: %lu", pid);

		HANDLE hProc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
		if (!hProc) {
			LOG("Failed to open process. Error: %lu", GetLastError());
			return false;
		}

		// Allocate and write payload
		void* remotePayload = VirtualAllocEx(
			hProc,
			nullptr,
			payload::size,
			MEM_COMMIT | MEM_RESERVE,
			PAGE_READWRITE
		);
		if (!remotePayload) {
			LOG("Failed to allocate memory for payload in target process. Error: %lu", GetLastError());
			CloseHandle(hProc);
			return false;
		}
		if (!WriteProcessMemory(hProc, remotePayload, payload::data, payload::size, nullptr)) {
			LOG("Failed to write payload to target process memory. Error: %lu", GetLastError());
			VirtualFreeEx(hProc, remotePayload, 0, MEM_RELEASE);
			CloseHandle(hProc);
			return false;
		}

		// Allocate and write params
		void* remoteParams = VirtualAllocEx(
			hProc,
			nullptr,
			sizeof(InjectionParams),
			MEM_COMMIT | MEM_RESERVE,
			PAGE_READWRITE
		);
		if (!remoteParams) {
			LOG("Failed to allocate memory in target process. Error: %lu", GetLastError());
			CloseHandle(hProc);
			return false;
		}

		InjectionParams params{};
		params.payload = reinterpret_cast<uint8_t*>(remotePayload);

		if (!WriteProcessMemory(hProc, remoteParams, &params, sizeof(params), nullptr)) {
			LOG("Failed to write params to target process memory. Error: %lu", GetLastError());
			VirtualFreeEx(hProc, remoteParams, 0, MEM_RELEASE);
			CloseHandle(hProc);
			return false;
		}

		// Allocate shellcode
		void* remoteShellcode = VirtualAllocEx(
			hProc,
			nullptr,
			0x1000,
			MEM_COMMIT | MEM_RESERVE,
			PAGE_EXECUTE_READWRITE
		);
		if (!remoteShellcode) {
			LOG("Failed to allocate memory for shellcode in target process. Error: %lu", GetLastError());
			VirtualFreeEx(hProc, remoteParams, 0, MEM_RELEASE);
			CloseHandle(hProc);
			return false;
		}

		// Write shellcode
		if (!WriteProcessMemory(hProc, remoteShellcode, detail::injectShellcode, 0x1000, nullptr)) {
			LOG("Failed to write shellcode to target process memory. Error: %lu", GetLastError());
			VirtualFreeEx(hProc, remoteShellcode, 0, MEM_RELEASE);
			VirtualFreeEx(hProc, remoteParams, 0, MEM_RELEASE);
			CloseHandle(hProc);
			return false;
		}

		// Create remote thread
		HANDLE hThread = CreateRemoteThread(
			hProc,
			nullptr,
			0,
			reinterpret_cast<LPTHREAD_START_ROUTINE>(remoteShellcode),
			remoteParams,
			0,
			nullptr
		);
		if (!hThread) {
			LOG("Failed to create remote thread. Error: %lu", GetLastError());
			VirtualFreeEx(hProc, remoteShellcode, 0, MEM_RELEASE);
			VirtualFreeEx(hProc, remoteParams, 0, MEM_RELEASE);
			CloseHandle(hProc);
			return false;
		}

		// Wait for thread to finish
		WaitForSingleObject(hThread, INFINITE);

		// Log result
		DWORD exitCode;
		if (GetExitCodeThread(hThread, &exitCode)) {
			LOG("Shellcode thread exited with code: 0x%08X", exitCode);
		}
		else {
			LOG("Failed to get thread exit code. Error: %lu", GetLastError());
		}

		return exitCode == static_cast<DWORD>(detail::ShellcodeError::Success);
	}

	namespace detail {

		void* allocString(HANDLE hProc, const std::string& str) {
			size_t sz = str.size() + 1;
			void* remoteStr = VirtualAllocEx(
				hProc,
				nullptr,
				sz,
				MEM_COMMIT | MEM_RESERVE,
				PAGE_READWRITE
			);

			if (!remoteStr) {
				LOG("Failed to allocate memory for string in target process. Error: %lu", GetLastError());
				return nullptr;
			}

			if (!WriteProcessMemory(hProc, remoteStr, str.c_str(), sz, nullptr)) {
				LOG("Failed to write string to target process memory. Error: %lu", GetLastError());
				VirtualFreeEx(hProc, remoteStr, 0, MEM_RELEASE);
				return nullptr;
			}

			return remoteStr;
		}

		ShellcodeError __stdcall injectShellcode(InjectionParams* params) {
			// Resolve mono
			HMODULE hMono = params->winapi.GetModuleHandleA(params->mono.mono_dll);
			if (!hMono) {
				return ShellcodeError::MonoNotFound;
			}

			// Manually resolve funcs
			RESOLVE_MONO_FUNC(mono_get_root_domain, ShellcodeError::MonoGetRootDomainNotFound);
			RESOLVE_MONO_FUNC(mono_thread_attach, ShellcodeError::MonoThreadAttachNotFound);
			RESOLVE_MONO_FUNC(mono_domain_assembly_open, ShellcodeError::MonoDomainAssemblyOpenNotFound);
			RESOLVE_MONO_FUNC(mono_assembly_get_image, ShellcodeError::MonoAssemblyGetImageNotFound);
			RESOLVE_MONO_FUNC(mono_assembly_load_from, ShellcodeError::MonoAssemblyLoadFromNotFound);
			RESOLVE_MONO_FUNC(mono_image_open_from_data, ShellcodeError::MonoImageOpenFromDataNotFound);
			RESOLVE_MONO_FUNC(mono_class_from_name, ShellcodeError::MonoClassFromNameNotFound);
			RESOLVE_MONO_FUNC(mono_class_get_method_from_name, ShellcodeError::MonoClassGetMethodFromNameNotFound);
			RESOLVE_MONO_FUNC(mono_runtime_invoke, ShellcodeError::MonoRuntimeInvokeNotFound);

			// Lets see
			void* domain = params->mono.mono_get_root_domain.ptr();
			params->mono.mono_thread_attach.ptr(domain);

			// Load image from data
			void* image = params->mono.mono_image_open_from_data.ptr(
				params->payload,
				payload::size,
				1,
				nullptr
			);
			if (!image) {
				return ShellcodeError::ImageOpenFromDataFailed;
			}

			// image->assembly is null when loaded from data
			void* assembly = params->mono.mono_assembly_load_from.ptr(
				image,
				params->mono.assembly_path,
				nullptr
			);
			if (!assembly) {
				return ShellcodeError::AssemblyLoadFromFailed;
			}

			image = params->mono.mono_assembly_get_image.ptr(assembly);
			if (!image) {
				return ShellcodeError::ImageOpenFromDataFailed;
			}

			void* clazz = params->mono.mono_class_from_name.ptr(
				image,
				params->mono.entrypoint_namespace,
				params->mono.entrypoint_class
			);
			if (!clazz) {
				return ShellcodeError::EntryClassNotFound;
			}

			void* method = params->mono.mono_class_get_method_from_name.ptr(
				clazz,
				params->mono.entrypoint_method,
				0
			);
			if (!method) {
				return ShellcodeError::EntryMethodNotFound;
			}

			void* result =  params->mono.mono_runtime_invoke.ptr(method, nullptr, nullptr, nullptr);
			if (!result || !*reinterpret_cast<bool*>(result)) {
				return ShellcodeError::EntryMethodInvokeFailed;
			}

			return ShellcodeError::Success;
		}

	} // namespace detail

} // namespace mrk