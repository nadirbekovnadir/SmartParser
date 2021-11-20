using System;

namespace SmartParser.MVVM.Commands.Common
{
	public abstract class BaseCommand : IBaseCommand
    {
        public virtual event EventHandler? CanExecuteChanged;
        public event EventHandler<Exception>? OnException;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            try
            {
                Execution(parameter);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
            }
        }

        public virtual void Execution(object? parameter)
        {
            throw new NotImplementedException();
        }

        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
