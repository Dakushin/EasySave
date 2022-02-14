using System.Windows.Input;

namespace EasySave.view.wpf.core;

public class CommandHandler : ICommand
{

    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public CommandHandler(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute.Invoke();
    }
    
    public void Execute(object? parameter)
    {
        _execute();
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}