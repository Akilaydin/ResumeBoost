using System.Runtime.InteropServices;

namespace OriApps.ResumeBoost;

static class Program
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    
    private static Mutex? _mutex;
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        if (TryPreventSecondInstance())
        {
            return;
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    private static bool TryPreventSecondInstance()
    {
        _mutex = new Mutex(true, "OriApps.ResumeBoost.Mutex", out bool createdNew);

        if (!createdNew)
        {
            IntPtr hWnd = FindWindow(null, "Resume Boost");
            if (hWnd != IntPtr.Zero)
            {
                PostMessage(hWnd, MainForm.WM_SHOWMAIN, IntPtr.Zero, IntPtr.Zero);
            }
            
            return true;
        }

        return false;
    }
}
