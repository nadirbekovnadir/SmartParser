using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public abstract class AsyncBaseCommand : IBaseCommand
    {
        private bool _isExecuting;
        private readonly Action<Exception> _onException;

        public bool IsExecuting
        {
            get {  return _isExecuting; }
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
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
            throw new NotImplementedException();
        }
    }
}
