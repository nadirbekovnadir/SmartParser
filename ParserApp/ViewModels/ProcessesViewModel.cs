using ParserApp.BindingParams;
using ParserApp.Commands;
using ParserApp.Interfaces;
using Models;
using System.ComponentModel;
using System.Windows.Input;
using ParserApp.Stores;
using System;
using System.IO;
using Models.Entities;

namespace ParserApp.VM
{
    public class ProcessesViewModel : BaseViewModel
    {
        public string ParseFolder { get; } = "Parse";
        public string ExceptFolder { get; } = "Except";
        public string FindFolder { get; } = "Find";

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

        #endregion


        #region Services

        private readonly IDialogService _dialogService;
        private readonly DataExtractor _dataExtractor;
        private readonly DataFinder _dataFinder;

        #endregion


        #region Stores

        private readonly ProcessStateStore _processStateStore;

        #endregion


        public ProcessesViewModel(
            ProcessStateStore processStateStore,
            IDialogService dialogService)
        {
            _processStateStore = processStateStore;
            _dialogService = dialogService;


            _dataExtractor = new DataExtractor();

            _dataExtractor.LoadingStarted += OnLoadingStarted;
            _dataExtractor.SourceLoaded += OnSourceLoaded;
            _dataExtractor.LoadingCompleted += OnLoadingCompleted;

            _dataExtractor.ParsingStarted += OnParsingStarted;
            _dataExtractor.SourceParsed += OnSourceParsed;
            _dataExtractor.ParsingCompleted += OnParsingCompleted;

            _dataExtractor.ProcessCompleted += OnProcessCompleted;


            _dataFinder = new DataFinder();

            _dataFinder.ProcessCompleted += OnFindingCompleted;
        }


        #region Event handlers


        private void OnLoadingStarted(object? sender, DataExtractor.LoadingEventArgs e)
        {
            _processStateStore.OnLoadingStarted(e.SourcesCount);
        }

        private void OnSourceLoaded(object? sender, DataExtractor.LoadedEventArgs e)
        {
            _processStateStore.OnSourceLoaded(e.Value, e.Success);
        }

        private void OnLoadingCompleted(object? sender, DataExtractor.LoadingEventArgs e)
        {
            _processStateStore.OnLoadingCompleted(e.SourcesCount);
        }

        private void OnParsingStarted(object? sender, DataExtractor.ParsingEventArgs e)
        {
            _processStateStore.OnParsingStarted(e.SourcesCount);
        }

        private void OnSourceParsed(object? sender, DataExtractor.ParsedEventArgs e)
        {
            _processStateStore.OnSourceParsed(e.Value, e.Success);
        }
        private void OnParsingCompleted(object? sender, DataExtractor.ParsingEventArgs e)
        {
            _processStateStore.OnParsingCompleted(e.SourcesCount, e.EntitiesCount);
        }

        private void OnProcessCompleted(object? sender, CompletedEventArgs e)
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
                _openWordsFile ??= new OpenWordsFileCommand(Pathes, _dialogService);
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
                    _dataExtractor,
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
                    _dataFinder,
                    (ex) => { });
                return _startFind;
            }
        }

        #endregion
    }
}
