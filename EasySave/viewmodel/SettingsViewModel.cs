using System.Collections.ObjectModel;
using EasySave.model;
using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class SettingsViewModel : ViewModelBase
{
    private readonly Model _model;
    private int _maximumFileSize;
    private string _sizeUnit = "Kb";
    
    public int MaximumFileSize
    {
        get => _maximumFileSize;
        set
        {
            _maximumFileSize = value;

            var unit = _sizeUnit switch
            {
                "B" => 1,
                "Kb" => 1024,
                "Mb" => 1024 * 1024,
                "Gb" => 1024 * 1024 * 1024
            };

            _model.fileSize = _maximumFileSize * unit;
            OnPropertyChanged();
        }
    }
    
    public string SizeUnit
    {
        get => _sizeUnit;
        set
        {
            _sizeUnit = value;
            MaximumFileSize = _maximumFileSize;
            OnPropertyChanged();
        }
    }

    public SettingsViewModel()
    {
        _model = Model.GetInstance();
        MaximumFileSize = 5;
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