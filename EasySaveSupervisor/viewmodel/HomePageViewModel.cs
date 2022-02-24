using EasySaveSupervisor.view.core;

namespace EasySaveSupervisor.viewmodel;

public class HomePageViewModel : ViewModelBase
{
    public string Username { get; } = Environment.UserName;
}