using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Entities;
using Models.Repositories;
using ParserApp.Interfaces;
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
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IContext, NewsContext>();
                    services.AddSingleton<IRepository<NewsEntity>, NewsRepository>();

                    services.AddTransient<IAutoExecutionCommandsService, AutoExecutionCommandsService>();
                    services.AddTransient<IDialogService, DefaultDialogService>();
                    services.AddTransient<IParserService, ParserService>();
                    services.AddTransient<IWordsProvider, WordsProvider>();

                    services.AddTransient<INewsExtractor, NewsExtractor>();
                    services.AddTransient<INewsFinder, NewsFinder>();

                    services.AddSingleton<ProcessStateStore>();
                    services.AddSingleton<WordsStore>();
                    services.AddSingleton<NewsStore>();

                    services.AddSingleton<ProcessesViewModel>();
                    services.AddSingleton<LogViewModel>();
                    services.AddSingleton<MainViewModel>();

                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });

                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();


            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
