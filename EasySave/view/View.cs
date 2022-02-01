using EasySave.viewmodel;

namespace EasySave.view;

internal abstract class View
{
    protected readonly ViewModel ViewModel;

    protected View()
    {
        ViewModel = new ViewModel(this);
    }

    public abstract void DisplayText(string text);

    public abstract void DisplayError(string text);

    public abstract void DisplaySuccess(string text);
}