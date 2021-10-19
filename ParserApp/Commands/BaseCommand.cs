using System;
using System.Windows.Input;

namespace ParserApp.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public virtual event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
