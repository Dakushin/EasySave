using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySave.model;

public enum SaveType //Enum for the type
{
    Complete,
    Differential
}

public class SaveWork : INotifyPropertyChanged
{
    //Private member data
    private SaveType _saveType;
    private string _sourcePath;
    private string _targetPath;
    private string _name;

    public SaveType SaveType
    {
        get => _saveType;
        set
        {
            _saveType = value;
            OnPropertyChanged();
        }
    }

    public string SourcePath
    {
        get => _sourcePath;
        set
        {
            _sourcePath = value;
            OnPropertyChanged();
        }
    }
    
    public string TargetPath
    {
        get => _targetPath;
        set
        {
            _targetPath = value;
            OnPropertyChanged();
        }
    }
    
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    //CONSTRUCTOR
    public SaveWork(string name, string sourcePath, string targetPath, SaveType saveType)
    {
        _name = name;
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _saveType = saveType;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}