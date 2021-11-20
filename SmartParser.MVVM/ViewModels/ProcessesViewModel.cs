using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartParser.Database.Repositories.Common;
using SmartParser.Domain.Entities;
using SmartParser.Domain.Services;
using SmartParser.Domain.Services.Common;
using SmartParser.MVVM.Commands;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Services.Common;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels.Common;
using SmartParser.MVVM.ViewModels.Parameters;
using System;

namespace SmartParser.MVVM.ViewModels
{
	public class ProcessesViewModel : BaseViewModel
    {
        #region Services

        private readonly IAutoExecutionCommandsService _autoExecutionCommandsService;
        private readonly IDialogService _dialogService;
        private readonly INewsExtractor _dataExtractor;
        private readonly INewsFinder _dataFinder;
        private readonly ILogger _logger;

        #endregion


        #region Repositories

        private readonly IRepository<NewsEntity> _newsRepo;

        #endregion


        #region Stores

        private readonly ProcessStateStore _processStateStore;
        private readonly WordsStore _wordsStore;
        private readonly NewsStore _newsStore;

        #endregion


        #region Binded params

        private PathesParams _pathes;
        public PathesParams Pathes
        {
            get => _pathes;
            set
            {
                _pathes = value;
                OnPropertyChanged(nameof(Pathes));
            }
        }


        private ParseParams _parse;
        public ParseParams Parse
        {
            get => _parse;
            set
            {
                _parse = value;
                OnPropertyChanged(nameof(Parse));
            }
        }

        private FindParams _find;
        public FindParams Find
        {
            get => _find;
            set
            {
                _find = value;
                OnPropertyChanged(nameof(Find));
            }
        }

        private AutoParams _auto;
        public AutoParams Auto
        {
            get => _auto;
            set
            {
                _auto = value;
                OnPropertyChanged(nameof(Auto));
            }
        }

        #endregion


        #region Commands

        public IBaseCommand OpenSitesFile { get; }
        public IBaseCommand OpenWordsFile { get; }
        public IBaseCommand OpenOutputDirectory { get; }
        public IBaseCommand StartParse { get; }
        public IBaseCommand StartFind { get; }
        public IBaseCommand StartAuto { get; }
        public IBaseCommand StopAuto { get; }

        #endregion


        #region Constructor

        public ProcessesViewModel(
            ProcessStateStore processStateStore,
            WordsStore wordsStore,
            NewsStore newsStore,

            IAutoExecutionCommandsService autoExecutionCommandsService,
            IDialogService dialogService,
            INewsExtractor newsExtractor,

            INewsFinder newsFinder,

            IRepository<NewsEntity> newsRepo,

            ILogger<ProcessesViewModel> logger,
            IServiceProvider loggerServices
            )
        {
            _processStateStore = processStateStore;
            _wordsStore = wordsStore;
            _newsStore = newsStore;

            _autoExecutionCommandsService = autoExecutionCommandsService;
            _dialogService = dialogService;

            _dataExtractor = newsExtractor;
            _dataFinder = newsFinder;

            _newsRepo = newsRepo;

            _logger = logger;


            Pathes = new PathesParams();
            Parse = new ParseParams();
            Find = new FindParams();
            Auto = new AutoParams();


            OpenSitesFile = new OpenSitesFileCommand(
                this, logger, dialogService);
            OpenWordsFile = new OpenWordsFileCommand(
                this, logger, wordsStore, dialogService);
            OpenOutputDirectory = new OpenOutputDirectoryCommand(
                this, logger, dialogService);

            StartParse = new StartParseCommand(
                this, logger, newsStore, newsExtractor, newsRepo);
            StartFind = new StartFindCommand(
                this, logger, wordsStore, newsStore, newsFinder);
            StartAuto = new StartAutoCommand(
                this, logger, autoExecutionCommandsService);
            StopAuto = new StopAutoCommand(
                this, logger, autoExecutionCommandsService);

            EventsSubscribe();
        }

        private void EventsSubscribe()
        {
            _dataExtractor.LoadingStarted += OnLoadingStarted;
            _dataExtractor.SourceLoaded += OnSourceLoaded;
            _dataExtractor.LoadingCompleted += OnLoadingCompleted;

            _dataExtractor.ParsingStarted += OnParsingStarted;
            _dataExtractor.SourceParsed += OnSourceParsed;
            _dataExtractor.ParsingCompleted += OnParsingCompleted;

            _dataExtractor.ProcessCompleted += OnExtractingCompleted;

            _dataFinder.ProcessCompleted += OnFindingCompleted;

            OpenSitesFile.OnException += onCommandException;
            OpenWordsFile.OnException += onCommandException;
            OpenOutputDirectory.OnException += onCommandException;

            StartParse.OnException += onCommandException;
            StartFind.OnException += onCommandException;
            StartAuto.OnException += onCommandException;
            StopAuto.OnException += onCommandException;
        }

        #endregion


        #region Event handlers


        private void OnLoadingStarted(object? sender, NewsExtractor.LoadingEventArgs e)
        {
            _processStateStore.OnLoadingStarted(e.SourcesCount);
        }

        private void OnSourceLoaded(object? sender, NewsExtractor.LoadedEventArgs e)
        {
            _processStateStore.OnSourceLoaded(e.Value, e.Success);
        }

        private void OnLoadingCompleted(object? sender, NewsExtractor.LoadingEventArgs e)
        {
            _processStateStore.OnLoadingCompleted(e.SourcesCount);
        }

        private void OnParsingStarted(object? sender, NewsExtractor.ParsingEventArgs e)
        {
            _processStateStore.OnParsingStarted(e.SourcesCount);
        }

        private void OnSourceParsed(object? sender, NewsExtractor.ParsedEventArgs e)
        {
            _processStateStore.OnSourceParsed(e.Value, e.Success);
        }
        private void OnParsingCompleted(object? sender, NewsExtractor.ParsingEventArgs e)
        {
            _processStateStore.OnParsingCompleted(e.SourcesCount, e.EntitiesCount);
        }

        private void OnExtractingCompleted(object? sender, CompletedEventArgs e)
        {
            
        }

        private void OnFindingCompleted(object? sender, CompletedEventArgs e)
        {
            
        }

        private void onCommandException(Object? sender, Exception e)
        {
            _logger.LogError(e, $"Failed: {sender?.GetType()}");
            _dialogService.ShowError(e.ToString());
        }

        #endregion
    }
}
