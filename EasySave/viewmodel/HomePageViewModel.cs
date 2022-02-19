using EasySave.view.wpf.core;

namespace EasySave.viewmodel;

public class HomePageViewModel : ViewModelBase
{
    public string Username { get; } = Environment.UserName;
}