#include "runtime.h"
#include "logger.h"

#include <Windows.h>

#define TOS2_PROC_NAME "TownOfSalem2.exe"

int main() {
	// This is tos2 so nothing fancy
	
	// Load up mono
	DWORD pid = mrk::getProcessPID(TOS2_PROC_NAME);
	if (pid == 0) {
		LOG("Failed to find ToS2 process. Make sure it's running and try again.");
		return 1;
	}

	if (!mrk::inject(pid)) {
		LOG("Injection failed.");
		return 1;
	}
	LOG("Injection successful!");

	return 0;
}