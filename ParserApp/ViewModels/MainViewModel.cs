using Models;
using Models.Repositories;
using ParserApp.BindingParams;
using ParserApp.Commands;
using ParserApp.Interfaces;
using ParserApp.Services;
using ParserApp.Stores;
using System.ComponentModel;
using System.Windows.Input;

namespace ParserApp.VM
{

    public class MainViewModel : BaseViewModel
    {
        public ProcessesViewModel ProcessesViewModel { get; }
        public LogViewModel LogViewModel { get; }

        // TODO узнать куда поместить инициализацию
        public MainViewModel()
        {
            ProcessStateStore processStateStore = new ProcessStateStore();

            ProcessesViewModel = new ProcessesViewModel(
                processStateStore,
                new DefaultDialogService(),
                new NewsRepository(new ExtractedContext()),
                new NewsRepository(new ExceptedContext()),
                new NewsRepository(new FindedContext()));

            LogViewModel = new LogViewModel(
                processStateStore);
        }
    }
}
