using System.Collections.ObjectModel;
using EasySave.view.wpf.core;
using EasySave.model;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    public ObservableCollection<string> PriorityFileExtensions { get; }

    public ObservableCollection<string> EncryptedFileExtensions { get; }
    
    public ObservableCollection<string> BusinessProcesses { get; }

    private readonly Model _model;

    public SettingsViewModel()
    {
        _model = Model.GetInstance();
        
        PriorityFileExtensions = new ObservableCollection<string>(new[]
        {
            ".pdf",
            ".txt",
            ".docx",
            ".pdf",
            ".txt",
            ".docx"
        });
        
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
        PriorityFileExtensions.Add(extension);
    }
    
    public void RemoveEncryptedFileExtension(string extension)
    {
        EncryptedFileExtensions.Remove(extension);
    }

    public void AddEncryptedFileExtension(string extension)
    {
        EncryptedFileExtensions.Add(extension);
    }

    public void RemoveBusinessProcess(string process)
    {
        BusinessProcesses.Remove(process);
    }

    public void AddBusinessProcess(string process)
    {
        BusinessProcesses.Add(process);
    }
}