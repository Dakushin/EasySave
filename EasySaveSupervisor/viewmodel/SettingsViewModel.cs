using System.Collections.ObjectModel;
using EasySaveSupervisor.model;
using EasySaveSupervisor.view.core;

namespace EasySaveSupervisor.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    private readonly Model _model;

    public SettingsViewModel()
    {
        _model = Model.GetInstance();


        EncryptedFileExtensions = new ObservableCollection<string>(new[]
        {
            ".pdf",
            ".txt",
            ".docx",
            ".mspaint"
        });

        BusinessProcesses = new ObservableCollection<string>(new[]
        {
            "notepad",
            "calc"
        });
    }

    public ObservableCollection<string> PriorityFileExtensions => _model.GetListPriorityExtension();

    public ObservableCollection<string> EncryptedFileExtensions { get; }

    public ObservableCollection<string> BusinessProcesses { get; }

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