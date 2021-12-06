using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

const string RegistryKeyRoot = @"SOFTWARE\Quick Browser Switcher";
const string SettingBrowserExec = "BrowserExec";
string RunLocation = "C:\\\\QuickBrowserSwitcher\\\\QuickBrowserSwitcher.exe";

const string ApplicationIconLink = @"C:\\Windows\\System32\\SHELL32.dll,14";

if (args.Length == 0)
{
    Console.WriteLine("Missing URI to redirect.");
}
else if (args.Length > 1)
{
    Console.WriteLine("Too many arguments.");
}
else if (args.Length == 1)
{
    if (args[0] == "--register")
    {
        Console.WriteLine("Registering app");

        RegistryKey registeredApps = Registry.CurrentUser.OpenSubKey(@"Software\RegisteredApplications", true);

        registeredApps.SetValue("Quick Browser Switcher", @"Software\Clients\StartMenuInternet\Quick Browser Switcher\Capabilities", RegistryValueKind.String);

        registeredApps.Close();

        RegistryKey browserSwitcher = Registry.CurrentUser.CreateSubKey(@"Software\Clients\StartMenuInternet\Quick Browser Switcher");
        browserSwitcher.SetValue("", "Quick Browser Switcher");

        var capabilities = browserSwitcher.CreateSubKey("Capabilities");

        capabilities.SetValue("ApplicationDescription", "The Quick Browser Switcher");
        capabilities.SetValue("ApplicationIcon", ApplicationIconLink);
        capabilities.SetValue("ApplicationName", "Quick Browser Switcher");

        var fileAssocations = capabilities.CreateSubKey("FileAssociations");
        fileAssocations.SetValue(".htm", "QuickBrowserSwitcher");
        fileAssocations.SetValue(".html", "QuickBrowserSwitcher");

        var startMenu = capabilities.CreateSubKey("Startmenu");
        startMenu.SetValue("StartMenuInternet", "QuickBrowserSwitcher");

        var urlAssociations = capabilities.CreateSubKey("URLAssociations");
        urlAssociations.SetValue("http", "QuickBrowserSwitcher");
        urlAssociations.SetValue("https", "QuickBrowserSwitcher");

        var defaultIcon = browserSwitcher.CreateSubKey("DefaultIcon");
        defaultIcon.SetValue("", ApplicationIconLink);

        var shellKey = browserSwitcher.CreateSubKey(@"shell\open\command");
        shellKey.SetValue("", RunLocation);

        capabilities.Close();

        RegistryKey classesKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\QuickBrowserSwitcher");
        classesKey.SetValue("", "QuickBrowserSwitcher Handler");

        var applicationKey = classesKey.CreateSubKey("Application");
        applicationKey.SetValue("AppUserModelId", "QuickBrowserSwitcher");
        applicationKey.SetValue("ApplicationIcon", ApplicationIconLink);
        applicationKey.SetValue("ApplicationName", "Quick Browser Switcher");
        applicationKey.SetValue("ApplicationDescription", "Free yourself!");
        applicationKey.SetValue("ApplicationCompany", "James Dearlove");

        var defaultIconKey = classesKey.CreateSubKey("DefaultIcon");
        defaultIconKey.SetValue("", ApplicationIconLink);

        var classesShell = classesKey.CreateSubKey(@"shell\open\command");
        classesShell.SetValue("", $"{RunLocation} \"%1\"");

        classesKey.Close();
    }
    else if (args[0] == "--unregister")
    {
        Console.WriteLine("Unregistering app");

        try
        {
            RegistryKey registeredApps = Registry.CurrentUser.OpenSubKey(@"Software\RegisteredApplications", true);

            registeredApps.DeleteValue("Quick Browser Switcher");
            registeredApps.Close();

            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Clients\StartMenuInternet\Quick Browser Switcher");

            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\QuickBrowserSwitcher");
        }
        catch (ArgumentException)
        {
            Console.Error.WriteLine("App is already unregistered");
        }

    }
    else
    {
        string browserShell = null;
        try
        {
            RegistryKey settingsKey = Registry.CurrentUser.OpenSubKey(RegistryKeyRoot);
            if (settingsKey != null)
            {
                var keyValue = settingsKey.GetValue(SettingBrowserExec);
                if (keyValue != null)
                {
                    browserShell = keyValue.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.Read();
        }

        if (browserShell == null)
        {
            Console.Error.WriteLine("The browser shell exec is null. Are you running the tray app?");
            Console.Read();
        }
        else
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(browserShell);
            processStartInfo.Arguments = args[0];
            processStartInfo.UseShellExecute = true;

            Process.Start(processStartInfo);
        }
    }
}