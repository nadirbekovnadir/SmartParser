using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels.Parameters;
using SmartParser.MVVM.ViewModels;

namespace SmartParser.MVVM.Commands
{
    public class OpenWordsFileCommand : BaseCommand
    {
        PathesParams _pathes;
        private WordsStore _wordsStore;
        IDialogService _dialogService;

        public OpenWordsFileCommand(
            ProcessesViewModel vm,
            WordsStore wordsStore,
            IDialogService dialogService)
        {
            _pathes = vm.Pathes;
            _wordsStore = wordsStore;
            _dialogService = dialogService;
        }

        public override void Execute(object? parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            _pathes.WordsFile = _dialogService.FilePath;
            _wordsStore.Load(_dialogService.FilePath);
        }
    }
}
