using Microsoft.Win32;
using System.Windows.Forms;

class Program
{
    const string RegistryKeyRoot = @"SOFTWARE\Quick Browser Switcher";
    const string SettingBrowserExec = "BrowserExec";

    static string currentBrowser = "NOT LOADED";
    static NotifyIcon trayIcon;

    static void trayIcon_MouseClick(object Sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            var regKey = Registry.CurrentUser.CreateSubKey(RegistryKeyRoot);
            var browserKey = regKey.GetValue(SettingBrowserExec);

            string setTo;
            if (browserKey == null || (string)browserKey == "msedge.exe")
            {
                setTo = "firefox.exe";
                trayIcon.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files\Mozilla Firefox\firefox.exe");
            }
            else
            {
                setTo = "msedge.exe";
                trayIcon.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
            }

            regKey.SetValue(SettingBrowserExec, setTo);
            regKey.Close();

            currentBrowser = setTo;
            trayIcon.Text = $"Current Browser: {currentBrowser}";

            Console.WriteLine($"Selected Browser: {setTo}");
        }
    }

    static void contextQuit_Click(object Sender, EventArgs e)
    {
        Application.Exit();
    }

    static void createTrayIcon()
    {
        trayIcon = new NotifyIcon();
        trayIcon.Text = $"Current Browser: {currentBrowser}";
        trayIcon.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files\Mozilla Firefox\firefox.exe");

        trayIcon.Visible = true;
        trayIcon.MouseClick += trayIcon_MouseClick;

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Quit", null, contextQuit_Click);
        trayIcon.ContextMenuStrip = contextMenu;
    }

    static void getCurrentBrowser()
    {
        var regKey = Registry.CurrentUser.CreateSubKey(RegistryKeyRoot);
        var browserKey = regKey.GetValue(SettingBrowserExec);

        if (browserKey == null)
        {
            currentBrowser = "Not set";
        }
        else
        {
            currentBrowser = browserKey.ToString();
        }
    }

    public static void Main(string[] args)
    {
        getCurrentBrowser();
        createTrayIcon();
        Console.WriteLine("Hello, World!");
        Application.Run();
    }
}

