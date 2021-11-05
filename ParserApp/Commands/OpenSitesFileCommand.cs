using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;

namespace SmartParser.MVVM.Commands
{
	public class OpenSitesFileCommand : BaseCommand
    {
        PathesParams _pathes;
        IDialogService _dialogService;

        public OpenSitesFileCommand(
            ProcessesViewModel vm, IDialogService dialogService)
        {
            _pathes = vm.Pathes;
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
