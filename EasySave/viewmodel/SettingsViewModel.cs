using System.Collections.ObjectModel;
using EasySave.view.wpf.core;
using EasySave.model;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    public ObservableCollection<string> PriorityFileExtensions => _model.GetListPriorityExtension();

    public ObservableCollection<string> EncryptedFileExtensions { get; }

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
}