using ParserApp.Params;
using ParserApp.Interfaces;
using System.IO;

namespace ParserApp.Commands
{
    public class OpenSitesFileCommand : BaseCommand
    {
        PathesParams _pathes;
        IDialogService _dialogService;

        public OpenSitesFileCommand(
            PathesParams pathes, IDialogService dialogService)
        {
            _pathes = pathes;
            _dialogService = dialogService;
        }

        public override void Execute(object? parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            _pathes.SitesFile = _dialogService.FilePath;
        }
    }
}
