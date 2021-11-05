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
        private AutoParams _autoParams;
        private IBaseCommand _startParseCommand;
        private IBaseCommand _startFindCommand;
        private IAutoExecutionCommandsService _autoExecutionService;

		public StartAutoCommand(
            ProcessesViewModel vm,
            IAutoExecutionCommandsService autoExecutionService) 
        {
            _autoParams = vm.Auto;
            _startParseCommand = vm.StartParse;
            _startFindCommand = vm.StartFind;
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
            List<ICommand> commands = new List<ICommand>();
            List<Object?> parameters = new List<Object?>();

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
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && !_autoParams.IsRunning;
        }
    }
}
