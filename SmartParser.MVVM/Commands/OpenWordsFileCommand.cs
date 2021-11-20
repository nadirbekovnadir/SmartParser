using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels.Parameters;
using SmartParser.MVVM.ViewModels;
using Microsoft.Extensions.Logging;
using System;

namespace SmartParser.MVVM.Commands
{
    public class OpenWordsFileCommand : BaseCommand
    {
        PathesParams _pathes;
        ILogger _logger;
        private WordsStore _wordsStore;
        IDialogService _dialogService;

        public OpenWordsFileCommand(
            ProcessesViewModel vm,
            ILogger logger,
            WordsStore wordsStore,
            IDialogService dialogService)
        {
            _pathes = vm.Pathes;
            _logger = logger;
            _wordsStore = wordsStore;
            _dialogService = dialogService;
        }

        public override void Execution(object? parameter)
        {
            if (!_dialogService.OpenFileDialog())
            {
                _logger.LogInformation("The file was not selected");

                return;
            }

            _pathes.WordsFile = _dialogService.FilePath;

            _logger.LogInformation($"Selected file: {_dialogService.FilePath}");

            _wordsStore.Load(_dialogService.FilePath);
            _logger.LogInformation("File with words loaded succesfully");
        }
    }
}
