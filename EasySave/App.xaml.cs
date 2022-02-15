using System.Windows;

namespace EasySave;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
        
        base.OnStartup(e);
    }
}