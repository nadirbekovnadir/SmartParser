using ParserApp.Params;
using ParserApp.Services;
using ParserApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParserApp.Commands
{
    public class StartAutoCommand : BaseCommand
    {
        private AutoParams _autoParams;
        private IBaseCommand _startParseCommand;
        private IBaseCommand _startFindCommand;
        private AutoExecutionCommandsService _autoExecutionService;

        public StartAutoCommand(
            ProcessesViewModel vm,
            AutoExecutionCommandsService autoExecutionService) 
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
