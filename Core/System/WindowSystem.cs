using System.Runtime.InteropServices;

namespace DoMulti.Core.System;

public class WindowSystem
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    [DllImport("user32.dll")]
    public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    public const uint SW_SHOW = 5;

    private int _index;
    private Organizer _organizer;

    public WindowSystem(Organizer o)
    {
        _index = 0;
        _organizer = o;
        SetForeground();
    }

    public void BringExternalWindowToFront(IntPtr hWnd)
    {
        uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
        uint appThread = GetCurrentThreadId();

        if (foreThread != appThread)
        {
            AttachThreadInput(foreThread, appThread, true);
            SetForegroundWindow(hWnd);
            AttachThreadInput(foreThread, appThread, false);
        }
        else
        {
            SetForegroundWindow(hWnd);
        }
    }

    private void checkAndFixIndex()
    {
        if (_organizer.AllChar.Count > 0)
        {
            if (_index == -1) _index = _organizer.AllChar.Count - 1;
            else if (_index >= _organizer.AllChar.Count)
            {
                _index = 0;
            }
        }
    }

    public void Next()
    {
        _index++;
        checkAndFixIndex();
        SetForeground();
    }

    public void Previous()
    {
        _index--;
        checkAndFixIndex();
        SetForeground();
    }

    private void SetForeground()
    {
        try
        {
            BringExternalWindowToFront(_organizer.AllChar[_index].Handle);
        }
        catch (ArgumentOutOfRangeException e)
        {
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}