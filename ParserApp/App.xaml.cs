using Models.Repositories;
using ParserApp.Services;
using ParserApp.Stores;
using ParserApp.ViewModels;
using System.Windows;

namespace ParserApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessStateStore processStateStore = new ProcessStateStore();
            NewsStore newsStore = new NewsStore();

            ProcessesViewModel processesViewModel = new ProcessesViewModel(
                processStateStore,
                newsStore,
                new DefaultDialogService(),
                new NewsRepository(new NewsContext()));

            LogViewModel logViewModel = new LogViewModel(
                processStateStore);

            MainViewModel mainViewModel = new MainViewModel(
                processesViewModel,
                logViewModel);

            MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
