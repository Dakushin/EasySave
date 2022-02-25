using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Gu.Localization;

namespace EasySave.view.wpf.windows;

/**
 * A view which displays the homepage.
 */
public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
    }

    /**
     * Executed when the user click on the 'show user doc' link.
     * Starts the web browser.
     */
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