#pragma once

// https://github.com/MRKDaGods/MRK-Android-Dev/blob/master/app/src/main/cpp/mrk/mono/MRKMonoCore.cpp
// https://github.com/MRKDaGods/rektmono/tree/master/injectionless_sex

#include <Windows.h>
#include <string>

#define DECLARE_WINAPI_FUNC(funcname) decltype(&funcname) funcname = &::funcname

#define DECLARE_STRING_MEMBER(name, str) char name[sizeof(str)] = str
#define DECLARE_MONO_MEMBER(membername) struct { \
    void*(*ptr)(...); \
    char name[sizeof(#membername)] = #membername; \
} membername

// Shellcode macros
#define RESOLVE_MONO_FUNC(func, err) do { \
	params->mono.func.ptr = reinterpret_cast<decltype(params->mono.func.ptr)>(params->winapi.GetProcAddress(hMono, params->mono.func.name)); \
	if (!params->mono.func.ptr) { \
		return err; \
	} \
} while(0)

namespace mrk {

	struct InjectionParams {
		struct {
			DECLARE_WINAPI_FUNC(MessageBoxA);
			DECLARE_WINAPI_FUNC(GetModuleHandleA);
			DECLARE_WINAPI_FUNC(GetProcAddress);
			DECLARE_WINAPI_FUNC(OutputDebugStringA);
			DECLARE_WINAPI_FUNC(wsprintfA);
		} winapi;

		// Mono lookups
		struct {
			DECLARE_STRING_MEMBER(mono_dll, "mono-2.0-bdwgc.dll");
			DECLARE_STRING_MEMBER(entrypoint_namespace, "MRK");
			DECLARE_STRING_MEMBER(entrypoint_class, "Entrypoint");
			DECLARE_STRING_MEMBER(entrypoint_method, "Main");
			DECLARE_STRING_MEMBER(assembly_path, "");

			// __int64 mono_get_root_domain()
			DECLARE_MONO_MEMBER(mono_get_root_domain);

			// __int64 mono_thread_attach(__int64 a1)
			DECLARE_MONO_MEMBER(mono_thread_attach);

			// __int64 mono_domain_assembly_open(__int64 a1, char *a2)
			DECLARE_MONO_MEMBER(mono_domain_assembly_open);

			// __int64 mono_assembly_get_image(__int64 a1)
			DECLARE_MONO_MEMBER(mono_assembly_get_image);

			// __int64 mono_assembly_load_from(__int64 a1, __int64 a2, char *a3)
			DECLARE_MONO_MEMBER(mono_assembly_load_from);

			// __int64 mono_image_open_from_data(__int64 a1, unsigned int a2, unsigned int a3, __int64 a4)
			DECLARE_MONO_MEMBER(mono_image_open_from_data);

			// __int64 mono_class_from_name(__int64 a1, char *a2, char *a3)
			DECLARE_MONO_MEMBER(mono_class_from_name);

			// __int64 mono_class_get_method_from_name(__int64 a1, char *a2, int a3)
			DECLARE_MONO_MEMBER(mono_class_get_method_from_name);

			// __int64 mono_runtime_invoke(__int64 a1, void *a2, void *a3, __int64 a4)
			DECLARE_MONO_MEMBER(mono_runtime_invoke);
		} mono;

		uint8_t* payload;
	};

	DWORD getProcessPID(const std::string& processName);
	bool inject(DWORD pid);

	namespace detail {

		enum class ShellcodeError : DWORD {
			Success = 0,
			MonoNotFound = 0x1,
			MonoGetRootDomainNotFound = 0x2,
			MonoThreadAttachNotFound = 0x3,
			MonoDomainAssemblyOpenNotFound = 0x4,
			MonoAssemblyGetImageNotFound = 0x5,
			MonoImageOpenFromDataNotFound = 0x6,
			MonoClassFromNameNotFound = 0x7,
			MonoClassGetMethodFromNameNotFound = 0x8,
			MonoRuntimeInvokeNotFound = 0x9,
			MonoAssemblyLoadFromNotFound = 0xA,

			// Other errors
			ImageOpenFromDataFailed = 0x101,
			EntryClassNotFound = 0x102,
			EntryMethodNotFound = 0x103,
			EntryMethodInvokeFailed = 0x104,
			AssemblyLoadFromFailed = 0x105,
		};

		void* allocString(HANDLE hProc, const std::string& str);
		ShellcodeError __stdcall injectShellcode(InjectionParams* params);

	} // namespace detail

} // namespace mrk