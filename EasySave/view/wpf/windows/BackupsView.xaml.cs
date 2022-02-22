using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasySave.model.backupStrategies;
using EasySave.view.wpf.core;
using EasySave.viewmodel;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace EasySave.view.wpf.windows;

public partial class BackupsView : UserControl
{
    private readonly BackupsViewModel _viewModel;

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

    private void OnChooseFolder(object sender, MouseButtonEventArgs e)
    {
        var folderBrowser = new OpenFileDialog
        {
            ValidateNames = false,
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Select a folder"
        };
        var result = folderBrowser.ShowDialog();
        if (!result.HasValue || !result.Value) return;

        switch (sender)
        {
            case TextBox textBox:
                textBox.Text = Path.GetDirectoryName(folderBrowser.FileName);
                break;
            case TextBlock textBlock:
                textBlock.Text = Path.GetDirectoryName(folderBrowser.FileName);
                break;
        }
    }

    private void OnCreateBackup(object sender, DialogClosingEventArgs e)
    {
        if (e.Parameter is not bool accept) return;

        if (accept)
        {
            if (BackupType.SelectedItem is not ComboBoxItem backupTypeSelected)
            {
                ViewModelBase.NotifyError(properties.Resources.Ask_Informations_Create_Backup);
                return;
            }

            var backupName = BackupName.Text.Trim();
            var backupSourcePath = BackupSourcePath.Text.Trim();
            var backupTargetPath = BackupTargetPath.Text.Trim();
            var backupType = backupTypeSelected.Name;

            if (backupName.Length > 0 && !string.IsNullOrEmpty(backupType))
                _viewModel.CreateBackup(backupName, backupSourcePath, backupTargetPath,
                    backupType.Equals("Complete") ? new Complete() : new Differential());
            else
                ViewModelBase.NotifyError(properties.Resources.Ask_Informations_Create_Backup);
        }
        else
        {
            ViewModelBase.NotifyInfo(properties.Resources.Cancelled);
        }
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
        _viewModel.ExecAllBackup();
    }
}