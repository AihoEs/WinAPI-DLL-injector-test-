using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Program;


[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;
}
class Program
{

    [DllImport("user32.dll",CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll",CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr WindowFromPoint(POINT point);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);



    public static void Main(string[] args)
    {
        while (true)
        {
            Thread.Sleep(3000);
                
            POINT cursorPos = default;
            GetCursorPos(out cursorPos);
            IntPtr handler = WindowFromPoint(cursorPos);

            var titleBuilder = new System.Text.StringBuilder(256);
            var classBuilder = new System.Text.StringBuilder(256);

            GetWindowText(handler, titleBuilder, titleBuilder.Capacity);
            GetClassName(handler, classBuilder, classBuilder.Capacity);

            Console.Clear();
            Console.WriteLine($"Дескриптор: {handler}");
            Console.WriteLine($"Название процесса: {titleBuilder}");
            Console.WriteLine($"Класс процесса: {classBuilder}");
            Console.WriteLine($"Координаты курсора: X = {cursorPos.X} Y = {cursorPos.Y}");

        }
        


    }

    





}