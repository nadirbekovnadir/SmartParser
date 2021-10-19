using ParserApp.BindingParams;
using ParserApp.Interfaces;
using System.IO;

namespace ParserApp.Commands
{
    public class OpenWordsFileCommand : BaseCommand
    {
        PathesParams _pathes;
        IDialogService _dialogService;

        public OpenWordsFileCommand(
            PathesParams pathes, IDialogService dialogService)
        {
            _pathes = pathes;
            _dialogService = dialogService;
        }

        public override void Execute(object? parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            _pathes.WordsFile = _dialogService.FilePath;
        }
    }
}
