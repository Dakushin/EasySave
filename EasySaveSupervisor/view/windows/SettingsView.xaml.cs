using System.Windows;
using System.Windows.Controls;
using EasySaveSupervisor.model;
using EasySaveSupervisor.view.core;
using EasySaveSupervisor.viewmodel;
using MaterialDesignThemes.Wpf;

namespace EasySaveSupervisor.view.windows;

public partial class SettingsView : UserControl
{
    private readonly SettingsViewModel _viewModel;

    public SettingsView()
    {
        InitializeComponent();
        _viewModel = (SettingsViewModel) DataContext;
    }

    private void OnSelectXml(object sender, RoutedEventArgs e)
    {
        _viewModel.ChangeLogFormat(new Xml());
    }

    private void OnSelectJson(object sender, RoutedEventArgs e)
    {
        _viewModel.ChangeLogFormat(new Json());
    }

    private void OnRemovePriorityFileExtension(object sender, RoutedEventArgs e)
    {
        if (HighPriorityListBox.SelectedItem is string extension)
        {
            _viewModel.RemovePriorityFileExtension(extension);
        }
    }

    private void OnRemoveEncryptedFileExtension(object sender, RoutedEventArgs e)
    {
        if (EncryptedListBox.SelectedItem is string extension)
        {
            _viewModel.RemoveEncryptedFileExtension(extension);
        }
    }

    private void OnAddExtension(object sender, DialogClosingEventArgs e)
    {
        if (Equals(e.Parameter, true))
        {
            var textBox = sender.Equals(PriorityDialog) ? PriorityTextBox : EncryptedTextBox;
            Action<string> action = sender.Equals(PriorityDialog) ? _viewModel.AddPriorityFileExtension : _viewModel.AddEncryptedFileExtension;
            
            if (!string.IsNullOrWhiteSpace(textBox.Text) && textBox.Text[0] == '.')
            {
                action(textBox.Text.Trim());
            }
            else
            {
                ViewModelBase.NotifyError(properties.Resources.Ask_Informations_Add_File_Extension);
            }
        }
        else
        {
            ViewModelBase.NotifyInfo(properties.Resources.Cancelled);
        }
    }

    private void OnAddBusinessProcess(object sender, DialogClosingEventArgs e)
    {
        if (Equals(e.Parameter, true))
        {
            if (!string.IsNullOrWhiteSpace(BusinessTextBox.Text))
            {
                _viewModel.AddBusinessProcess(BusinessTextBox.Text.Trim());
            }
            else
            {
                ViewModelBase.NotifyError(properties.Resources.Ask_Informations_Business_Process);
            }
        }
        else
        {
            ViewModelBase.NotifyInfo(properties.Resources.Cancelled);
        }
    }

    private void OnRemoveBusinessProcess(object sender, RoutedEventArgs e)
    {
        if (BusinessListBox.SelectedItem is string process)
        {
            _viewModel.RemoveBusinessProcess(process);
        }
    }
}