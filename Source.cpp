#include <windows.h>



DWORD WINAPI MainThread(LPVOID lpParam)
{

    MessageBoxW(
        NULL,
        L"DLL Inject success!",
        L"Status",
        MB_OK | MB_ICONINFORMATION
    );

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    if (ul_reason_for_call == DLL_PROCESS_ATTACH)
    {
        DisableThreadLibraryCalls(hModule);
        CreateThread(NULL, 0, MainThread, NULL, 0, NULL);
    }
    return TRUE;
}