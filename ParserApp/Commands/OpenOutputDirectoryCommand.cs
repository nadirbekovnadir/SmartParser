using ParserApp.BindingParams;
using ParserApp.Interfaces;
using System.IO;

namespace ParserApp.Commands
{
    public class OpenOutputDirectoryCommand : BaseCommand
    {
        PathesParams _pathes;
        IDialogService _dialogService;

        public OpenOutputDirectoryCommand(
            PathesParams pathes, IDialogService dialogService)
        {
            _pathes = pathes;
            _dialogService = dialogService;
        }

        public override void Execute(object? parameter)
        {
            if (!_dialogService.OpenFolderDialog())
                return;

            _pathes.Output = _dialogService.FolderPath;
        }
    }
}
