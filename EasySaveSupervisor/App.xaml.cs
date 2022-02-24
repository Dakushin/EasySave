using System.Globalization;
using System.Windows;

namespace EasySaveSupervisor;

public partial class App : Application
{
    private static readonly Mutex Singleton = new(true, "EasySaveSupervisor");

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!Singleton.WaitOne(TimeSpan.Zero, true))
        {
            MessageBox.Show("An EasySave supervisor instance is already running");
            Current.Shutdown(0);
        }

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

        base.OnStartup(e);
    }
}