using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Gu.Localization;

namespace EasySave.view.wpf.windows;

public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
    {
        var currentCulture = Translator.CurrentCulture;
        var userDocUrl = $"https://sachathommet.fr/easysave/user_doc_{currentCulture.Name}.pdf";

        var processInfo = new ProcessStartInfo(userDocUrl);
        processInfo.UseShellExecute = true;
        Process.Start(processInfo);
        e.Handled = true;
    }
}