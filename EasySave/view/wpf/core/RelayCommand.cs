using System.Windows.Input;

namespace EasySave.view.wpf.core;

public class RelayCommand : ICommand
{

    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return parameter != null && _canExecute(parameter);
    }
    
    public void Execute(object? parameter)
    {
        if (parameter != null) _execute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}