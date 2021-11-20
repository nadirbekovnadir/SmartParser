using Microsoft.Extensions.Logging;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System;

namespace SmartParser.MVVM.Commands
{
	public class OpenSitesFileCommand : BaseCommand
    {
        private readonly PathesParams _pathes;
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;


        public OpenSitesFileCommand(
            ProcessesViewModel vm,
            ILogger logger,
            IDialogService dialogService)
        {
            _pathes = vm.Pathes;
            _dialogService = dialogService;
            _logger = logger;
        }

        public override void Execution(object? parameter)
        {
            if (!_dialogService.OpenFileDialog())
            {
                _logger.LogInformation("The file was not selected");
                return;
            }

            _pathes.SitesFile = _dialogService.FilePath;

            _logger.LogInformation($"Selected file: {_dialogService.FilePath}");
        }
    }
}
