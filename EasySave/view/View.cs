using EasySave.viewmodel;

namespace EasySave.view;

internal abstract class View
{
    //PRIVATE VARIABLE
    protected readonly ViewModel ViewModel;

    //CONSTRUCTOR
    protected View()
    {
        ViewModel = new ViewModel(this);
    }
    //ABSTRACT FUNCTION
    public abstract void DisplayText(string text);

    public abstract void DisplayError(string text);

    public abstract void DisplaySuccess(string text);
}