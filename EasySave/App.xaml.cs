using System.Globalization;
using System.Windows;

namespace EasySave;

public partial class App : Application
{
    private static readonly Mutex Singleton = new(true, "EasySave");

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!Singleton.WaitOne(TimeSpan.Zero, true))
        {
            MessageBox.Show("An EasySave instance is already running");
            Current.Shutdown(0);
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

        base.OnStartup(e);
    }
}