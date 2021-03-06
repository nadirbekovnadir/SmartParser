using System;
using System.Windows.Input;

namespace SmartParser.MVVM.Commands.Common
{
	public class RelayCommand<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Action<T> _execute;
        public Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute is null || _canExecute((T)parameter);
        }

        public void Execute(object? parameter) => _execute((T)parameter);
    }
}
