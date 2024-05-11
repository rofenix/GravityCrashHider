using GravityCrashHider;
using System.Diagnostics;

partial class Program
{
    private static readonly string GravityCrashWindowTitle = "Gravity(tm) Error Handle";
    private static readonly TimeSpan RefreshRate = TimeSpan.FromSeconds(1);
    private static readonly HashSet<int> PatchedProcesses = [];
    private static Process[]? CurrentRagProcesses;

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
        while (true)
        {
            // Get every ragnarok process
            CurrentRagProcesses = Process.GetProcessesByName("ragexe");

            // Scan for windows when any ragnarok process is not yet patched
            if (CurrentRagProcesses.Length > 0 && !AreCurrentProcessesPatched(CurrentRagProcesses))
            {
                // Remove unused ragnarok processes in case we have closed them
                RemoveUnusedProcesses(CurrentRagProcesses);

                // Enumerate all windows
                WinImports.EnumWindows(ScanWindow, 0);
            }
            await Task.Delay(RefreshRate);
        }
    }

    private static bool ScanWindow(nint handle, nint param)
    {
        // Is the window visible?
        if (WinImports.IsWindowVisible(handle))
        {
            // Does it belong to any of the ragnarok processes?
            WinImports.GetWindowThreadProcessId(handle, out var processId);
            if (CurrentRagProcesses is not null && CurrentRagProcesses.Any(p => p.Id == processId))
            {
                // Is it the gravity crash window?
                if (IsGravityCrashWindow(handle))
                {
                    // Minimize and hide
                    WinImports.SendMessage(handle, WinImports.WM_SYSCOMMAND, WinImports.SC_MINIMIZE, 0);
                    WinImports.ShowWindow(handle, WinImports.SW_HIDE);
                    Console.WriteLine("> Gravity crash handler detected and is now hidden !");
                    PatchedProcesses.Add(processId);
                }
            }
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

    private static bool AreCurrentProcessesPatched(Process[] ragProcesses)
    {
        return ragProcesses.Length == PatchedProcesses.Count && ragProcesses.All(p => PatchedProcesses.Contains(p.Id));
    }

    private static void RemoveUnusedProcesses(Process[] ragProcesses)
    {
        for (int i = PatchedProcesses.Count - 1; i >= 0; i--)
        {
            var processId = PatchedProcesses.ElementAt(i);
            if (!ragProcesses.Any(p => p.Id == processId))
            {
                PatchedProcesses.Remove(processId);
            }
        }
    }
}
