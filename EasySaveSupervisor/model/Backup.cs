using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySaveSupervisor.model;

public class Backup : INotifyPropertyChanged
{
    //Private member data
    private string _backupStrategy;
    private bool _crypted;
    private string _name;
    private int _progression;
    private string _sourcePath;
    private string _targetPath;

    //CONSTRUCTOR
    public Backup(string name, string sourcePath, string targetPath, string backupStrategy,
        bool crypted, int progression)
    {
        _name = name;
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _backupStrategy = backupStrategy;
        _progression = progression;
        _crypted = crypted;
    }

    public string BackupStrategyName
    {
        get => _backupStrategy;
        set
        {
            _backupStrategy = value;
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

    public int Progression
    {
        get => _progression;
        set
        {
            if (value is >= 0 and <= 100)
            {
                _progression = value;
                OnPropertyChanged();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The progression must be between 0 and 100");
            }
        }
    }

    public bool Crypted
    {
        get => _crypted;
        set
        {
            _crypted = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}