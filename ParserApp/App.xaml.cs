using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Models.Entities;
using Models.Repositories;
using ParserApp.Interfaces;
using ParserApp.Services;
using ParserApp.Stores;
using ParserApp.ViewModels;
using Serilog;
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
                .UseSerilog((host, loggerConfig) =>
                {
                    loggerConfig
                        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                        .WriteTo.Debug()
                        .MinimumLevel.Information(); // MinimumLevel.Override
                })
                .ConfigureServices(services =>
                {
                    AddRepositories(services);
                    AddServices(services);
                    AddStores(services);
                    AddViewModels(services);

                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });

                })
                .Build();
        }


        #region HostBuilder methods

        private static void AddViewModels(IServiceCollection services)
        {
            services.AddSingleton<ProcessesViewModel>();
            services.AddSingleton<LogViewModel>();
            services.AddSingleton<MainViewModel>();
        }

        private static void AddStores(IServiceCollection services)
        {
            services.AddSingleton<ProcessStateStore>();
            services.AddSingleton<WordsStore>();
            services.AddSingleton<NewsStore>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAutoExecutionCommandsService, AutoExecutionCommandsService>();
            services.AddTransient<IDialogService, DefaultDialogService>();
            services.AddTransient<IParserService, ParserService>();
            services.AddTransient<IWordsProvider, WordsProvider>();

            services.AddTransient<INewsExtractor, NewsExtractor>();
            services.AddTransient<INewsFinder, NewsFinder>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IContext, NewsContext>();
            services.AddSingleton<IRepository<NewsEntity>, NewsRepository>();
        }

        #endregion


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
