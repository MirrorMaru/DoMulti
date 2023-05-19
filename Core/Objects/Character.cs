using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DoMulti.Core.Objects;

public class Character
{
    private string name;
    private int initiative;
    private IntPtr handle;
    private string version;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindow(string classname, string windowName);

    public Character(string n, int i, string v)
    {
        name = n;
        initiative = i;
        version = v;

        string windowName = name + " - Dofus " + version;

        Process[] processes = Process.GetProcesses();

        foreach (var VARIABLE in processes)
        {
            IntPtr windowHandle = FindWindow(null, windowName);

            if (windowHandle != IntPtr.Zero)
            {
                Console.WriteLine(name + " window found !");
                handle = windowHandle;
            }
            else
            {
                Console.Error.WriteLine(name + "window NOT FOUND ! Ignoring ... ");
            }
        }
    }

    public string Name => name;

    public int Initiative => initiative;

    public nint Handle => handle;
}