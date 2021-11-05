using System.Windows.Input;

namespace SmartParser.MVVM.Commands.Common
{
	public interface IBaseCommand : ICommand
    {
        public void RaiseCanExecuteChanged();
    }
}
