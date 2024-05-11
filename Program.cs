using GravityCrashHider;

partial class Program
{
    private static readonly string GravityCrashWindowTitle = "Gravity(tm) Error Handle";
    private static readonly TimeSpan RefreshRate = TimeSpan.FromSeconds(1);
    private static bool Hidden = false;

    static void Main(string[] _)
    {
        Console.WriteLine("> Gravity crash hider");
        Console.WriteLine("> Made by Fenix");
        Console.WriteLine("> Join the iRO Spooky Market: https://discord.gg/PQwpDmRSuM");
        Console.WriteLine("> Scanner started, dont close this program...");
        Task.Factory.StartNew(async () => await Start(), TaskCreationOptions.LongRunning);
        Console.ReadLine();
    }

    private static async Task Start()
    {
        // Scan windows till i find it
        while (!Hidden)
        {
            WinImports.EnumWindows(ScanWindow, 0);
            await Task.Delay(RefreshRate);
        }
    }

    private static bool ScanWindow(nint handle, nint param)
    {
        // Is the dialog visible and is the gravity crash handle?
        if (WinImports.IsWindowVisible(handle) && IsGravityCrashWindow(handle))
        {
            // Minimize and hide
            WinImports.SendMessage(handle, WinImports.WM_SYSCOMMAND, WinImports.SC_MINIMIZE, 0);
            WinImports.ShowWindow(handle, WinImports.SW_HIDE);
            Console.WriteLine("> Gravity crash handler detected and is now hidden !");
            Hidden = true;
        }
        return true;
    }

    private static bool IsGravityCrashWindow(nint handle)
    {
        int titleLength = WinImports.GetWindowTextLength(handle);
        char[] titleBuff = new char[titleLength];

        if (WinImports.GetWindowText(handle, titleBuff, titleLength) <= 0)
        {
            return false;
        }

        var title = new string(titleBuff, 0, titleLength - 1);
        return title == GravityCrashWindowTitle;
    }
}
