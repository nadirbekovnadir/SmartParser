using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System.ComponentModel;

namespace SmartParser.MVVM.Commands
{
	public class StopAutoCommand : BaseCommand
    {
        private AutoParams _autoParams;
        private IAutoExecutionCommandsService _autoExecutionService;

        public StopAutoCommand(
            ProcessesViewModel vm,
            IAutoExecutionCommandsService autoExecutionService)
        {
            _autoParams = vm.Auto;
            _autoExecutionService = autoExecutionService;

            _autoParams.PropertyChanged += AutoParams_PropertyChanged;
        }

        private void AutoParams_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AutoParams.IsRunning))
                return;

            RaiseCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            _autoExecutionService.Stop();
            _autoParams.IsRunning = _autoExecutionService.IsRunning;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _autoParams.IsRunning;
        }
    }
}
