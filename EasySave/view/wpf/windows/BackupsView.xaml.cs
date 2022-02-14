using System.Windows;
using System.Windows.Controls;
using EasySave.viewmodel;

namespace EasySave.view.wpf.windows;

public partial class BackupsView : UserControl
{
    private BackupsViewModel _viewModel;
    
    public BackupsView()
    {
        InitializeComponent();
        
        _viewModel = new BackupsViewModel();
        DataContext = _viewModel;
    }

    private void OnDeleteBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.DeleteSelectedBackup();
    }
    
    private void OnExecuteBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.ExecuteSelectedBackup();
    }
}