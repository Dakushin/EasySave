using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EasySave.view.wpf.windows;

public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
    {
        var processInfo = new ProcessStartInfo("https://google.fr");
        processInfo.UseShellExecute = true;
        Process.Start(processInfo);
        e.Handled = true;
    }
}