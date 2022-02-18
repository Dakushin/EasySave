using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    
    private readonly SettingsViewModel _settingsViewModel;
    private readonly BackupsViewModel _backupsViewModel;
    private readonly HomePageViewModel _homePageViewModel;

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

    public MainViewModel()
    {
        _settingsViewModel = new SettingsViewModel();
        _backupsViewModel = new BackupsViewModel(null);
        _homePageViewModel = new HomePageViewModel();

        OnNavigateToHomePage = new CommandHandler(NavigateToHomePageView);
        OnNavigateToBackups = new CommandHandler(NavigateToBackupsView);
        OnNavigateToSettings = new CommandHandler(NavigateToSettingsView);

        _currentViewModel = _backupsViewModel;
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