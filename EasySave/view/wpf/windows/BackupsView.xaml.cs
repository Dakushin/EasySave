using System.Windows.Controls;
using EasySave.viewmodel;

namespace EasySave.view.wpf.windows;

public partial class BackupsView : UserControl
{
    public BackupsView()
    {
        InitializeComponent();
        DataContext = new BackupsViewModel(null);
    }
}