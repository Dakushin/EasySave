using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    
    private readonly SettingsViewModel _settingsViewModel;
    private readonly BackupsViewModel _backupsViewModel;

    public CommandHandler OnNavigateToBackups { get; set; }
    public CommandHandler OnNavigateToSettings { get; set; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        _settingsViewModel = new SettingsViewModel();
        _backupsViewModel = new BackupsViewModel(null);

        OnNavigateToBackups = new CommandHandler(NavigateToBackupsView);
        OnNavigateToSettings = new CommandHandler(NavigateToSetttingsView);

        _currentViewModel = _backupsViewModel;
    }

    private void NavigateToBackupsView()
    {
        CurrentViewModel = _backupsViewModel;
    }

    private void NavigateToSetttingsView()
    {
        CurrentViewModel = _settingsViewModel;
    }
}