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
    public abstract void DisplayText(string text);

    public abstract void DisplayError(string text);

    public abstract void DisplaySuccess(string text);
}