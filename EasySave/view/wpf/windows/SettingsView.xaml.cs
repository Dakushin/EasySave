using System.Windows.Controls;
using EasySave.viewmodel;

namespace EasySave.view.wpf.windows;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}