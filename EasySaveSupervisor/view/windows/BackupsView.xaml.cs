using System.Windows;
using System.Windows.Controls;
using EasySaveSupervisor.viewmodel;

namespace EasySaveSupervisor.view.windows;

/**
 * A view which displays all the backups.
 */
public partial class BackupsView : UserControl
{
    private readonly BackupsViewModel _viewModel;

    public BackupsView()
    {
        InitializeComponent();

        _viewModel = DataContext as BackupsViewModel;
    }

    private void OnFilter(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox filterBox) return;

        _viewModel.Filter(filterBox.Text);
    }

    private void OnResumeBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.ResumeSelectedBackup();
    }

    private void OnPauseBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.PauseSelectedBackup();
    }

    private void OnCancelBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.CancelSelectedBackup();
    }

    private void OnExecuteAllBackups(object sender, RoutedEventArgs e)
    {
        _viewModel.ExecuteAllBackups();
    }

    private void OnExecuteBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.ExecuteSelectedBackup();
    }
}