using ParserApp.Params;
using ParserApp.Services;
using ParserApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StopAutoCommand : BaseCommand
    {
        private AutoParams _autoParams;
        private AutoExecutionCommandsService _autoExecutionService;

        public StopAutoCommand(
            ProcessesViewModel vm,
            AutoExecutionCommandsService autoExecutionService)
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
