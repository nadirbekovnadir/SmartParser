using ParserApp.BindingParams;
using ParserApp.Commands;
using ParserApp.Interfaces;
using Models;
using System.ComponentModel;
using System.Windows.Input;
using ParserApp.Stores;
using System;

namespace ParserApp.VM
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

        #endregion


        #region Services

        private readonly IDialogService _dialogService;
        private readonly DataExtractor _dataExtractor;

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

        #endregion


        #region Commands

        private ICommand _openSitesFile;
        public ICommand OpenSitesFile
        {
            get
            {
                _openSitesFile ??= new OpenSitesFileCommand(Pathes, _dialogService);
                return _openSitesFile;
            }
        }

        private ICommand _openWordsFile;
        public ICommand OpenWordsFile
        {
            get
            {
                _openWordsFile ??= new OpenWordsFileCommand(Pathes, _dialogService);
                return _openWordsFile;
            }
        }

        private ICommand _openOutputDirectory;
        public ICommand OpenOutputDirectory
        {
            get
            {
                _openOutputDirectory ??= new OpenOutputDirectoryCommand(Pathes, _dialogService);
                return _openOutputDirectory;
            }
        }

        private ICommand _startParse;
        public ICommand StartParse
        {
            get
            {
                _startParse ??= new StartParseCommand(
                    Parse, Pathes, _dataExtractor);
                return _startParse;
            }
        }

        #endregion
    }
}
