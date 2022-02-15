using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;

namespace EasySave.view.wpf.core;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public static SnackbarMessageQueue SnackBarMessageQueue { get; } = new(TimeSpan.FromMilliseconds(2500));

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void NotifyInfo(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon { Kind = PackIconKind.Information}, () => {});
    }

    protected void NotifyError(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon { Kind = PackIconKind.Error}, () => {});
    }

    protected void NotifySuccess(string message)
    {
        SnackBarMessageQueue.Enqueue(message, new PackIcon { Kind = PackIconKind.CheckBold}, () => {});
    }
}