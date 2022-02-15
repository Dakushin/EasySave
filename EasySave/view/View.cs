using EasySave.viewmodel;

namespace EasySave.view;

public abstract class View
{
    //PRIVATE VARIABLE
    protected readonly BackupsViewModel BackupsViewModel;

    //CONSTRUCTOR
    protected View()
    {
        BackupsViewModel = new BackupsViewModel(this);
    }
    //ABSTRACT FUNCTION
    public abstract void NotifyInfo(string message);

    public abstract void NotifyError(string message);

    public abstract void NotifySuccess(string message);
}