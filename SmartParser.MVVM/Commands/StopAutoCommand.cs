using Microsoft.Extensions.Logging;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System;
using System.ComponentModel;

namespace SmartParser.MVVM.Commands
{
	public class StopAutoCommand : BaseCommand
    {
        private readonly AutoParams _autoParams;
        private readonly ILogger _logger;
        private readonly IAutoExecutionCommandsService _autoExecutionService;

        public StopAutoCommand(
            ProcessesViewModel vm,
            ILogger logger,
            IAutoExecutionCommandsService autoExecutionService)
        {
            _autoParams = vm.Auto;

            _logger = logger;

            _autoExecutionService = autoExecutionService;

            _autoParams.PropertyChanged += AutoParams_PropertyChanged;
        }

        private void AutoParams_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AutoParams.IsRunning))
                return;

            RaiseCanExecuteChanged();
        }

        public override void Execution(object? parameter)
        {
            _logger.LogInformation("StopAuto started");

            _autoExecutionService.Stop();
            _autoParams.IsRunning = _autoExecutionService.IsRunning;

            _logger.LogInformation("StopAuto ended");
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _autoParams.IsRunning;
        }
    }
}
