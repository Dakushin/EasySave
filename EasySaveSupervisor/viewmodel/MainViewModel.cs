using EasySaveSupervisor.model;
using EasySaveSupervisor.view.core;

namespace EasySaveSupervisor.viewmodel;

public class MainViewModel : ViewModelBase
{
    private readonly BackupsViewModel _backupsViewModel;
    private readonly HomePageViewModel _homePageViewModel;
    private readonly SettingsViewModel _settingsViewModel;

    private ViewModelBase _currentViewModel;

    public MainViewModel()
    {
        _backupsViewModel = new BackupsViewModel();
        Client.GetInstance().SetBackupsViewModel(_backupsViewModel);
        _homePageViewModel = new HomePageViewModel();
        _settingsViewModel = new SettingsViewModel();

        OnNavigateToHomePage = new CommandHandler(NavigateToHomePageView);
        OnNavigateToBackups = new CommandHandler(NavigateToBackupsView);
        OnNavigateToSettings = new CommandHandler(NavigateToSettingsView);

        _currentViewModel = _homePageViewModel;
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