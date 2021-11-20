using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SmartParser.MVVM.Commands.Common
{
	public abstract class AsyncBaseCommand : IBaseCommand
    {
        private bool _isExecuting;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        public bool IsExecuting
        {
            get {  return _isExecuting; }
            set
            {
                _dispatcher.Invoke(
                    () =>
                    {
                        _isExecuting = value;
                        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    });
            }
        }

        public event EventHandler? CanExecuteChanged;
        public event EventHandler<Exception>? OnException;

        public virtual bool CanExecute(object? parameter)
        {
            return !IsExecuting;
        }

        public virtual async void Execute(object? parameter)
        {
            await ExecuteAsync(parameter);
        }

        public virtual async Task ExecuteAsync(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await Execution(parameter);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
            }
            IsExecuting = false;
        }

        protected abstract Task Execution(object? parameter);

        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
