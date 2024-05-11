using System.Runtime.InteropServices;

namespace GravityCrashHider;

public partial class WinImports
{
    public const uint WM_SYSCOMMAND = 0x0112;
    public const int SC_MINIMIZE = 0xF020;
    public const int SW_HIDE = 0;

    public delegate bool EnumWindowsHandler(nint handle, nint lParam);

    [LibraryImport("User32.dll", EntryPoint = "GetWindowTextW", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int GetWindowText(nint handle, [Out] char[] lpString, int nMaxCount);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowTextLengthA", SetLastError = true)]
    public static partial int GetWindowTextLength(nint handle);

    [LibraryImport("user32.dll", EntryPoint = "SendMessageA")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SendMessage(nint handle, uint Msg, nint wParam, nint lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ShowWindow(nint handle, int nCmdShow);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool EnumWindows(EnumWindowsHandler lpEnumFunc, nint lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool IsWindowVisible(nint handle);

    [LibraryImport("user32.dll")]
    public static partial uint GetWindowThreadProcessId(nint hWnd, out int processId);
}
