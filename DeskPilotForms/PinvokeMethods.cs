using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DeskPilotForms
{

    class PinvokeMethods
    {
        // Define delegates
        public delegate bool CallBackEW(int hwnd, int lParam);

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        // Import unmanaged signatures
        [DllImport("user32")]
        public static extern int EnumWindows(CallBackEW x, int y);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        // Define callback function 
        public static bool Report(int hwnd, int lParam)
        {
            Console.Write("Window handle is ");
            Console.WriteLine(hwnd);
            return true;
        }

        // main function
        public static void EnumWindowsMain()
        {
            CallBackEW myCallBack = new CallBackEW(PinvokeMethods.Report);
            EnumWindows(myCallBack, 0);
        }

        public static string GetText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

    }

}
