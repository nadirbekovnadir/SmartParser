using SmartParser.MVVM.ViewModels.Common;

namespace SmartParser.MVVM.ViewModels
{

    public class MainViewModel : BaseViewModel
    {
        public ProcessesViewModel ProcessesViewModel { get; }
        public LogViewModel LogViewModel { get; }

        // TODO узнать куда поместить инициализацию
        public MainViewModel(
            ProcessesViewModel processesViewModel,
            LogViewModel logViewModel)
        {
            ProcessesViewModel = processesViewModel;
            LogViewModel = logViewModel;
        }
    }
}
