using System.Collections.ObjectModel;
using EasySave.model;
using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    private readonly Model _model;

    public SettingsViewModel()
    {
        _model = Model.GetInstance();
    }

    public ObservableCollection<string> PriorityFileExtensions => _model.GetListPriorityExtension();
    
    public ObservableCollection<string> EncryptedFileExtensions => _model.GetListExtentionToCheck();

    public ObservableCollection<string> BusinessProcesses => _model.GetListProcessToCheck();

    public void ChangeLogFormat(FileFormat fileFormat)
    {
        _model.SetLogFileFormat(fileFormat);
    }

    public void RemovePriorityFileExtension(string extension)
    {
        PriorityFileExtensions.Remove(extension);
    }

    public void AddPriorityFileExtension(string extension)
    {
        if (!PriorityFileExtensions.Contains(extension)) PriorityFileExtensions.Add(extension);
    }

    public void RemoveEncryptedFileExtension(string extension)
    {
        EncryptedFileExtensions.Remove(extension);
    }

    public void AddEncryptedFileExtension(string extension)
    {
        if (!EncryptedFileExtensions.Contains(extension)) EncryptedFileExtensions.Add(extension);
    }

    public void RemoveBusinessProcess(string process)
    {
        BusinessProcesses.Remove(process);
    }

    public void AddBusinessProcess(string process)
    {
        if (!BusinessProcesses.Contains(process)) BusinessProcesses.Add(process);
    }
}