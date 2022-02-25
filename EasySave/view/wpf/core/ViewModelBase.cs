using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace EasySave.view.wpf.core;

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
        Application.Current.Dispatcher.Invoke(() => 
            SnackBarMessageQueue.Enqueue(message, 
                new PackIcon {Kind = PackIconKind.Information}, () => { })
        );
    }

    /**
     * Display an error in the view.
     */
    public static void NotifyError(string message)
    {
        Application.Current.Dispatcher.Invoke(() => 
            SnackBarMessageQueue.Enqueue(message, 
                new PackIcon {Kind = PackIconKind.Error}, () => { })
        );
    }

    /**
     * Display a success message in the view.
     */
    public static void NotifySuccess(string message)
    {
        Application.Current.Dispatcher.Invoke(() => 
            SnackBarMessageQueue.Enqueue(message, 
                new PackIcon {Kind = PackIconKind.CheckBold}, () => { })
        );
    }
}