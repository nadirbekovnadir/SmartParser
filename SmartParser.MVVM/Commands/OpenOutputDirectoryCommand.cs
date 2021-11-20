using Microsoft.Extensions.Logging;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System;

namespace SmartParser.MVVM.Commands
{
	public class OpenOutputDirectoryCommand : BaseCommand
    {
        private readonly PathesParams _pathes;
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;


        public OpenOutputDirectoryCommand(
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
            if (!_dialogService.OpenFolderDialog())
            {
                _logger.LogInformation("The folder was not selected");
                return;
            }

            _pathes.Output = _dialogService.FolderPath;

            _logger.LogInformation($"Selected folder: {_dialogService.FolderPath}");
        }
    }
}
