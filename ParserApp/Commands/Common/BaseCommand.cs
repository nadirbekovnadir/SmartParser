using System;

namespace SmartParser.MVVM.Commands.Common
{
	public abstract class BaseCommand : IBaseCommand
    {
        public virtual event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
