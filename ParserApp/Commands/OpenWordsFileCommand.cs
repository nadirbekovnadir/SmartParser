using ParserApp.Params;
using ParserApp.Interfaces;
using ParserApp.Stores;
using System.IO;

namespace ParserApp.Commands
{
    public class OpenWordsFileCommand : BaseCommand
    {
        PathesParams _pathes;
        private WordsStore _wordsStore;
        IDialogService _dialogService;

        public OpenWordsFileCommand(
            PathesParams pathes,
            WordsStore wordsStore,
            IDialogService dialogService)
        {
            _pathes = pathes;
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
