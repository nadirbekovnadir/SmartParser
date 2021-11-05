using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;

namespace SmartParser.MVVM.Commands
{
	public class OpenOutputDirectoryCommand : BaseCommand
    {
        PathesParams _pathes;
        IDialogService _dialogService;

        public OpenOutputDirectoryCommand(
            ProcessesViewModel vm, IDialogService dialogService)
        {
            _pathes = vm.Pathes;
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
