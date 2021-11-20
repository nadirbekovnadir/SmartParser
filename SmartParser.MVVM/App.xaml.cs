using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParserApp;
using Serilog;
using SmartParser.Database.Contexts;
using SmartParser.Database.Contexts.Common;
using SmartParser.Database.Repositories;
using SmartParser.Database.Repositories.Common;
using SmartParser.Domain.Entities;
using SmartParser.Domain.Services;
using SmartParser.Domain.Services.Common;
using SmartParser.MVVM.Commands;
using SmartParser.MVVM.Services;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels;
using System;
using System.Windows;

namespace SmartParser.MVVM
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
                        .Enrich.FromLogContext()
                        .WriteTo.File(
                            "log.txt", 
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {SourceContext} - {Message}{NewLine}{Exception}")
                        .WriteTo.Debug()
                        .MinimumLevel.Information(); // MinimumLevel.Override
                })
                .ConfigureServices(services =>
                {
                    AddRepositories(services);
                    AddServices(services);
                    AddStores(services);
                    AddViewModels(services);
                    AddCommands(services);

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

        private void AddCommands(IServiceCollection services)
        {

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
