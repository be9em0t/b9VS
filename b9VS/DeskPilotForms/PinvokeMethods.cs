using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DeskPilotForms
{

  class PinvokeMethods
  {
    // Delegates
    public delegate bool CallBackEnumWin(int hwnd, int lParam);

    private delegate bool CallBackEnumChildWin(IntPtr hwnd, IntPtr lParam);

    // Signatures
    [DllImport("user32")]
    public static extern int EnumWindows(CallBackEnumWin x, int y);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(IntPtr window, CallBackEnumChildWin callback, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);

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
      CallBackEnumWin myCallBack = new CallBackEnumWin(PinvokeMethods.Report);
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

    public static bool IsVisible(IntPtr hWnd)
    {
      bool result = IsWindowVisible(hWnd);
      return result;
    }
  }

}
