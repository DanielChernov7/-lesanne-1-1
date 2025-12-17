using System;
using System.Windows.Input;

namespace Uni1Tools.Services;

public sealed class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether the command can execute.
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    /// <summary>
    /// Executes the command action.
    /// </summary>
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// Raises the CanExecuteChanged event to refresh command state.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
