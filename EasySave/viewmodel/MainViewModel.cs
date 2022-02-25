using EasySave.model;
using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class MainViewModel : ViewModelBase
{
    //PRIVATE VARIABLE
    private readonly BackupsViewModel _backupsViewModel;
    private readonly HomePageViewModel _homePageViewModel;

    private readonly SettingsViewModel _settingsViewModel;
    private ViewModelBase _currentViewModel;

    //CONSTRUCTOR
    public MainViewModel()
    {
        _settingsViewModel = new SettingsViewModel();
        _backupsViewModel = new BackupsViewModel();
        Server.GetInstance().SetBackupsViewModel(_backupsViewModel);
        _homePageViewModel = new HomePageViewModel();

        OnNavigateToHomePage = new CommandHandler(NavigateToHomePageView);
        OnNavigateToBackups = new CommandHandler(NavigateToBackupsView);
        OnNavigateToSettings = new CommandHandler(NavigateToSettingsView);

        _currentViewModel = _homePageViewModel;
    }

    //COMMAND HANDLER TO NAVIGATE IN THE APP
    public CommandHandler OnNavigateToBackups { get; set; }
    public CommandHandler OnNavigateToSettings { get; set; }
    public CommandHandler OnNavigateToHomePage { get; set; }

    public ViewModelBase CurrentViewModel //PUBLIC VARIABLE that get the current viewmodel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    private void NavigateToHomePageView() //Function to go on homepage
    {
        CurrentViewModel = _homePageViewModel;
    }

    private void NavigateToBackupsView() //Function to go on BackupView
    {
        CurrentViewModel = _backupsViewModel;
    }

    private void NavigateToSettingsView()  //Function to go on Settings
    {
        CurrentViewModel = _settingsViewModel;
    }
}