using System.Runtime.InteropServices;
using System.Text;

namespace DoMulti.Core.System;

public class VersionFinder
{
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    public string GetVersion()
    {
        string found = "";
        EnumWindows((hWnd, lParam) =>
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(hWnd, sb, sb.Capacity);

            if (sb.ToString().Contains("- Dofus "))
            {
                found = sb.ToString().Split(' ')[^1];
                return false;
            }

            return true;
        }, 0);
        return found;
    }
}