using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    
    private void OnCreateBackup(object sender, RoutedEventArgs e)
    {
        _viewModel.Todo();
    }

    private void OnChooseFolder(object sender, MouseButtonEventArgs e)
    {
        if (sender is not TextBlock folderTextBlock) return;

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
            folderTextBlock.Text = Path.GetDirectoryName(folderBrowser.FileName); 
        }
    }

    private void DataGrid_OnInitializingNewItem(object sender, InitializingNewItemEventArgs e)
    {
        SystemSounds.Beep.Play();
    }
}