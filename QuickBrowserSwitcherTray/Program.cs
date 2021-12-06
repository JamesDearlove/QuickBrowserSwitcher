using Microsoft.Win32;
using System.Windows.Forms;

class Program
{
    const string RegistryKeyRoot = @"SOFTWARE\Quick Browser Switcher";
    const string SettingBrowserExec = "BrowserExec";

    static NotifyIcon trayIcon = new();
    static ToolStripMenuItem switchItem = new();

    private static void trayIcon_MouseClick(object Sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            switchBrowsers();
        }
    }

    private static void SwitchItem_Click(object? sender, EventArgs e)
    {
        switchBrowsers();
    }

    static void switchBrowsers()
    {
        var regKey = Registry.CurrentUser.CreateSubKey(RegistryKeyRoot);
        var browserKey = regKey.GetValue(SettingBrowserExec);

        string setTo;
        if (browserKey == null || (string)browserKey == "msedge.exe")
        {
            setTo = "firefox.exe";
        }
        else
        {
            setTo = "msedge.exe";
        }

        regKey.SetValue(SettingBrowserExec, setTo);
        regKey.Close();

        updateTray(setTo);

        Console.WriteLine($"Selected Browser: {setTo}");
    }

    static void contextQuit_Click(object Sender, EventArgs e)
    {
        Application.Exit();
    }

    static void createTrayIcon()
    {
        trayIcon = new NotifyIcon();
        trayIcon.Visible = true;
        trayIcon.MouseClick += trayIcon_MouseClick;

        updateTray();

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(switchItem);
        switchItem.Click += SwitchItem_Click;

        contextMenu.Items.Add("Quit", null, contextQuit_Click);

        trayIcon.ContextMenuStrip = contextMenu;
    }

    static void updateTray()
    {
        var regKey = Registry.CurrentUser.CreateSubKey(RegistryKeyRoot);
        var browserKey = regKey.GetValue(SettingBrowserExec);

        if (browserKey != null)
        {
            updateTray((string)browserKey);
        }

    }

    static void updateTray(string browser)
    {
        trayIcon.Text = $"Current Browser: {browser}";

        switch (browser)
        {
            case "firefox.exe":
                trayIcon.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files\Mozilla Firefox\firefox.exe");
                switchItem.Text = "Switch to msedge.exe";
                break;
            case "msedge.exe":
                trayIcon.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
                switchItem.Text = "Switch to firefox.exe";
                break;
            default:
                break;
        }
    }

    public static void Main(string[] args)
    {
        createTrayIcon();
        Console.WriteLine("Hello, World!");
        Application.Run();
    }
}

