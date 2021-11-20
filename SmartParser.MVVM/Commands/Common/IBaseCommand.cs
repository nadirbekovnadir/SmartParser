using System;
using System.Windows.Input;

namespace SmartParser.MVVM.Commands.Common
{
	public interface IBaseCommand : ICommand
    {
        public event EventHandler<Exception>? OnException;
        public void RaiseCanExecuteChanged();
    }
}
