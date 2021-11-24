using Microsoft.Extensions.Logging;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace SmartParser.MVVM.Commands
{
	public class StartAutoCommand : BaseCommand
    {
        private readonly AutoParams _autoParams;
        private readonly IBaseCommand _startParseCommand;
        private readonly IBaseCommand _startFindCommand;
        private readonly ILogger _logger;
        private readonly IAutoExecutionCommandsService _autoExecutionService;

		public StartAutoCommand(
            ProcessesViewModel vm,
            ILogger logger,
            IAutoExecutionCommandsService autoExecutionService)
        {
            _autoParams = vm.Auto;
            _startParseCommand = vm.StartParse;
            _startFindCommand = vm.StartFind;

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
            _logger.LogInformation("Auto started");

            List<ICommand> commands = new List<ICommand>();
            List<object?> parameters = new List<object?>();

            if (_autoParams.MustParse)
            {
                commands.Add(_startParseCommand);
                parameters.Add(null);
            }

            if (_autoParams.MustFind)
            {
                commands.Add(_startFindCommand);
                parameters.Add(null);
            }

            _autoExecutionService.Start(commands, parameters, TimeSpan.FromMinutes(_autoParams.DelayMinutes));
            _autoParams.IsRunning = _autoExecutionService.IsRunning;

            _logger.LogInformation("Auto ended");
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_autoParams.IsRunning;
        }
    }
}
