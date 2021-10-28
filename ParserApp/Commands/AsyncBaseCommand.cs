using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ParserApp.Commands
{
    public abstract class AsyncBaseCommand : IBaseCommand
    {
        private bool _isExecuting;
        private readonly Action<Exception> _onException;
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

        public AsyncBaseCommand(Action<Exception> onException)
        {
            _onException = onException;
        }

        public virtual bool CanExecute(object? parameter)
        {
            return !IsExecuting;
        }

        public virtual async void Execute(object? parameter)
        {
            await ExecuteWithTask(parameter);
        }

        public virtual async Task ExecuteWithTask(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object? parameter);

        public virtual void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
