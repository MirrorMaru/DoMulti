using System.Runtime.InteropServices;
using System.Text;

namespace DoMulti.Core.System;

public class CharacterFinder
{
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    private Organizer _organizer;

    public CharacterFinder(Organizer o)
    {
        _organizer = o;
    }

    public List<string> GetCharacters()
    {
        List<String> characters = new List<string>();
        EnumWindows((hWnd, lParam) =>
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(hWnd, sb, sb.Capacity);

            if (sb.ToString().Contains("- Dofus "+_organizer.Version))
            {
                characters.Add(sb.ToString().Split(' ')[0]);
            }
            return true;
        }, 0);
        return characters;
    }
}