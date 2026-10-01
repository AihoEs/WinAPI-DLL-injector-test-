using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows;
using static Program;
using System.ComponentModel.Design;



class Program
{

    
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        VMRead = 0x0010,
        VMWrite = 0x0020,
        VMOperation = 0x0008,
        QueryInformation = 0x0400
    }

    [Flags]
    public enum AllocationType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000
    }

    [Flags]
    public enum MemoryProtect : uint
    {
        ReadWrite = 0x04
    }


    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(ProcessAccessFlags dwAccess, bool unherbit, uint PID);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr baseAddress, uint dwSize, AllocationType type, MemoryProtect protect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hProcess);



    public static void Main(string[] args)
    {
        ProcessAccessFlags AccessFlag = ProcessAccessFlags.All;
        Process[] processes = Process.GetProcessesByName("notepad");
        if (processes.Length == 0)
        {
            Console.WriteLine("Откройте файл");
            return;
        }
        Process process = processes[0];
        uint PID = (uint)process.Id;
        IntPtr handler = OpenProcess(AccessFlag, false, PID);


        string DLLPath = @"C:\Users\ASUS\source\repos\ConsoleApp2\x64\Debug\Project2.dll";
        byte[] pathBytes = Encoding.Unicode.GetBytes(DLLPath + "\0");
        uint VirtualSize = (uint)pathBytes.Length;
        IntPtr baseAddress = IntPtr.Zero;
        AllocationType type = AllocationType.Commit | AllocationType.Reserve;
        MemoryProtect memory = MemoryProtect.ReadWrite;
        IntPtr VirtualAccessHandler = VirtualAllocEx(handler,baseAddress,VirtualSize,type,memory);


        nint WriteBaseAddress = (nint)VirtualAccessHandler;
        int nSize = pathBytes.Length;
        IntPtr BytesWritten;
        if(WriteProcessMemory(handler,WriteBaseAddress,pathBytes,nSize,out BytesWritten))
        {
            IntPtr moduleHandle = GetModuleHandle("kernel32.dll");
            IntPtr ProcAddressHandle = GetProcAddress(moduleHandle, "LoadLibraryW");
            IntPtr ThreadId;

            IntPtr Threads = CreateRemoteThread(handler, IntPtr.Zero, 0, ProcAddressHandle, VirtualAccessHandler, 0, out ThreadId);

            CloseHandle(Threads);
        }
        
        CloseHandle(handler);
        







    }

    



   


}