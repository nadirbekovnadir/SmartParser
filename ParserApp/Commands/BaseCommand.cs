using System;
using System.Windows;
using System.Windows.Input;

namespace ParserApp.Commands
{
    public abstract class BaseCommand : IBaseCommand
    {
        public virtual event EventHandler? CanExecuteChanged;

        public virtual void RaiseCanExecuteChanged()
        {
            throw new NotImplementedException();
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
