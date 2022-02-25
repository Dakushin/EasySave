using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class HomePageViewModel : ViewModelBase
{
    public string Username { get; } = Environment.UserName; //public variable that get the name of the user in the OS
}