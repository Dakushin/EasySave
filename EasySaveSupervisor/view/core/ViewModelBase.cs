using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;

namespace EasySaveSupervisor.view.core;

/**
 * Base class for all viewmodel. Allow binding and observation between the view and the viewmodel. 
 */
public class ViewModelBase : INotifyPropertyChanged
{
    public static SnackbarMessageQueue SnackBarMessageQueue { get; } = new(TimeSpan.FromSeconds(2));
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /**
     * Display an info in the view.
     */
    public static void NotifyInfo(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon {Kind = PackIconKind.Information}, () => { });
    }
}