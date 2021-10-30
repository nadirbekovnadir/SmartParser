using ParserApp.Params;
using ParserApp.Commands;
using ParserApp.Interfaces;
using Models;
using System.ComponentModel;
using System.Windows.Input;
using ParserApp.Stores;
using System;
using System.IO;
using Models.Entities;
using Models.Repositories;
using ParserApp.Services;
using Microsoft.Extensions.Logging;

namespace ParserApp.ViewModels
{
    public class ProcessesViewModel : BaseViewModel
    {
        #region Binded params

        private PathesParams _pathes;
        public PathesParams Pathes
        {
            get
            {
                _pathes ??= new PathesParams();
                return _pathes;
            }
            set
            {
                _pathes = value;
                OnPropertyChanged(nameof(Pathes));
            }
        }


        private ParseParams _parse;
        public ParseParams Parse
        {
            get
            {
                _parse ??= new ParseParams();
                return _parse;
            }
            set
            {
                _parse = value;
                OnPropertyChanged(nameof(Parse));
            }
        }
        
        private FindParams _find;
        public FindParams Find
        {
            get
            {
                _find ??= new FindParams();
                return _find;
            }
            set
            {
                _find = value;
                OnPropertyChanged(nameof(Find));
            }
        }

        private AutoParams _auto;

        public AutoParams Auto
        {
            get 
            {
                _auto ??= new AutoParams();
                return _auto; 
            }
            set 
            {
                _auto = value; 
            }
        }


        #endregion


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


        #region Constructor

        public ProcessesViewModel(
            ProcessStateStore processStateStore,
            WordsStore wordsStore,
            NewsStore newsStore,

            IAutoExecutionCommandsService autoExecutionCommandsService,
            IDialogService dialogService,
            INewsExtractor  newsExtractor,

            INewsFinder newsFinder,
            ILogger<ProcessesViewModel> logger,

            IRepository<NewsEntity> newsRepo
            )
        {
            _processStateStore = processStateStore;
            _wordsStore = wordsStore;
            _newsStore = newsStore;

            _autoExecutionCommandsService = autoExecutionCommandsService;
            _dialogService = dialogService;
            _logger = logger;

            _dataExtractor = newsExtractor;
            _dataFinder = newsFinder;

            _newsRepo = newsRepo;

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

        #endregion


        #region Commands

        private IBaseCommand _openSitesFile;
        public IBaseCommand OpenSitesFile
        {
            get
            {
                _openSitesFile ??= new OpenSitesFileCommand(Pathes, _dialogService);
                return _openSitesFile;
            }
        }

        private IBaseCommand _openWordsFile;
        public IBaseCommand OpenWordsFile
        {
            get
            {
                _openWordsFile ??= new OpenWordsFileCommand(
                    Pathes,
                    _wordsStore,
                    _dialogService);
                return _openWordsFile;
            }
        }

        private IBaseCommand _openOutputDirectory;
        public IBaseCommand OpenOutputDirectory
        {
            get
            {
                _openOutputDirectory ??= new OpenOutputDirectoryCommand(Pathes, _dialogService);
                return _openOutputDirectory;
            }
        }

        private IBaseCommand _startParse;
        public IBaseCommand StartParse
        {
            get
            {
                _startParse ??= new StartParseCommand(
                    this,
                    _newsStore,
                    _dataExtractor,
                    _newsRepo,
                    (ex) => { });
                return _startParse;
            }
        }

        
        private IBaseCommand _startFind;
        public IBaseCommand StartFind
        {
            get
            {
                _startFind ??= new StartFindCommand(
                    this,
                    _wordsStore,
                    _newsStore,
                    _dataFinder,
                    (ex) => { });
                return _startFind;
            }
        }

        private IBaseCommand _startAuto;
        public IBaseCommand StartAuto
        {
            get
            {
                _startAuto ??= new StartAutoCommand(
                    this,
                    _autoExecutionCommandsService);
                return _startAuto;
            }
        }

        private IBaseCommand _stopAuto;
        public IBaseCommand StopAuto
        {
            get
            {
                _stopAuto ??= new StopAutoCommand(
                    this,
                    _autoExecutionCommandsService);
                return _stopAuto;
            }
        }

        #endregion
    }
}
