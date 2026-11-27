using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Inno.Testing.EditorJourney;

internal static class EditorJourneyExit
{
    internal static bool Request(int process)
    {
        List<nint> windows = [];
        if (!EnumWindows((
            window,
            state
        ) =>
        {
            GetWindowThreadProcessId(window, out uint owner);
            if (owner == (uint)process)
            {
                var title = new StringBuilder(256);
                GetWindowText(window, title, title.Capacity);
                if (title.ToString() == "Inno Editor")
                {
                    windows.Add(window);
                }
            }
            return true;
        }, nint.Zero)
        ) {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        if (windows.Count == 0)
        {
            return false;
        }
        if (windows.Count != 1)
        {
            throw new InvalidOperationException("The owned Editor has an ambiguous primary window.");
        }
        GetWindowThreadProcessId(windows[0], out uint currentOwner);
        if (currentOwner != (uint)process)
        {
            throw new InvalidOperationException("The Editor window ownership changed before close.");
        }
        if (!PostMessage(windows[0], 0x0010, nint.Zero, nint.Zero))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        return true;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumWindows(
        WindowCallback callback,
        nint state
    );

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(
        nint window,
        out uint process
    );

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowText(
        nint window,
        StringBuilder text,
        int maximum
    );

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool PostMessage(
        nint window,
        uint message,
        nint first,
        nint second
    );

    private delegate bool WindowCallback(
        nint window,
        nint state
    );
}
