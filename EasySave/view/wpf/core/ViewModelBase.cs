using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;

namespace EasySave.view.wpf.core;

public class ViewModelBase : INotifyPropertyChanged
{
    public static SnackbarMessageQueue SnackBarMessageQueue { get; } = new(TimeSpan.FromSeconds(2));
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static void NotifyInfo(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon {Kind = PackIconKind.Information}, () => { });
    }

    public static void NotifyError(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon {Kind = PackIconKind.Error}, () => { });
    }

    public static void NotifySuccess(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon {Kind = PackIconKind.CheckBold}, () => { });
    }
}