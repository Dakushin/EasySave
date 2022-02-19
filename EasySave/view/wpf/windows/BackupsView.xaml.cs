using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EasySave.model;
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
        if (sender is not TextBox textBox) return;

        var folderBrowser = new OpenFileDialog
        {
            ValidateNames = false,
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Select a folder"
        };
        var result = folderBrowser.ShowDialog(); 
        if (result.HasValue && result.Value) 
        { 
            textBox.Text = Path.GetDirectoryName(folderBrowser.FileName); 
        }
    }

    private void OnCreateBackup(object sender, DialogClosingEventArgs e)
    {
        if (e.Parameter is not bool accept) return;

        if (accept)
        {
            if (BackupType.SelectedItem is not ComboBoxItem backupTypeSelected)
            {
                ViewModelBase.NotifyError("Please, fill all the information when creating a backup");
                return;
            }
                
            var backupName = BackupName.Text.Trim();
            var backupSourcePath = BackupSourcePath.Text.Trim();
            var backupTargetPath = BackupTargetPath.Text.Trim();
            var backupType = backupTypeSelected.Name;

            if (backupName.Length > 0 && backupType != null && Enum.TryParse(backupType, out SaveType saveType))
            {
                _viewModel.CreateSaveWork(backupName, backupSourcePath, backupTargetPath, saveType);
            }
            else
            {
                ViewModelBase.NotifyError("Please, fill all the information when creating a backup");
            }
        }
        else
        {
            ViewModelBase.NotifyInfo("Operation cancelled");
        }
    }

    private void OnFilter(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox filterBox) return;
        
        _viewModel.Filter(filterBox.Text);
    }
}