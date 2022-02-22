using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasySave.model.backupStrategies;

namespace EasySave.model;

public class Backup : INotifyPropertyChanged
{
    //Private member data
    private BackupStrategy _backupStrategy;
    private string _name;
    private int _progression;
    private string _sourcePath;
    private string _targetPath;
    private bool _crypted;

    //CONSTRUCTOR
    public Backup(string name, string sourcePath, string targetPath, BackupStrategy backupStrategy)
    {
        _name = name;
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _backupStrategy = backupStrategy;
        _progression = 0;
    }

    public BackupStrategy BackupStrategy
    {
        get => _backupStrategy;
        set
        {
            _backupStrategy = value;
            OnPropertyChanged();
        }
    }

    public string BackupStrategyName
    {
        get => _backupStrategy.GetName();
        set
        {
            _backupStrategy = value switch
            {
                "Complete" => new Complete(),
                "Differential" => new Differential(),
                _ => throw new ArgumentException("the backup strategy must be either \"complete\" or \"differential\"")
            };
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

    public bool Execute()
    {
        return _backupStrategy.Execute(this);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}