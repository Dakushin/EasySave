using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class MainViewModel : ViewModelBase
{
    private readonly BackupsViewModel _backupsViewModel;
    private readonly HomePageViewModel _homePageViewModel;

    private readonly SettingsViewModel _settingsViewModel;
    private ViewModelBase _currentViewModel;

    public MainViewModel()
    {
        _settingsViewModel = new SettingsViewModel();
        _backupsViewModel = new BackupsViewModel();
        _homePageViewModel = new HomePageViewModel();

        OnNavigateToHomePage = new CommandHandler(NavigateToHomePageView);
        OnNavigateToBackups = new CommandHandler(NavigateToBackupsView);
        OnNavigateToSettings = new CommandHandler(NavigateToSettingsView);

        _currentViewModel = _backupsViewModel;
    }

    public CommandHandler OnNavigateToBackups { get; set; }
    public CommandHandler OnNavigateToSettings { get; set; }
    public CommandHandler OnNavigateToHomePage { get; set; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    private void NavigateToHomePageView()
    {
        CurrentViewModel = _homePageViewModel;
    }

    private void NavigateToBackupsView()
    {
        CurrentViewModel = _backupsViewModel;
    }

    private void NavigateToSettingsView()
    {
        CurrentViewModel = _settingsViewModel;
    }
}