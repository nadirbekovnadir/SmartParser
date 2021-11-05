using SmartParser.Domain.Entities;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels.Common;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Threading;

namespace SmartParser.MVVM.ViewModels
{
	public class LogViewModel : BaseViewModel
    {
        #region Bindings

        private string _processInfo;
        public string ProcessInfo 
        { 
            get => _processInfo; 
            set
            {
                _processInfo = value;
                OnPropertyChanged(nameof(ProcessInfo));
            } 
        }

        private int _maxObjects = 100;
        public int MaxObjects
        {
            get => _maxObjects;
            set
            {
                _maxObjects = value;
                OnPropertyChanged(nameof(MaxObjects));
            }
        }

        private int _currentObjects = 0;
        public int CurrentObjects
        {
            get => _currentObjects;
            set
            {
                _currentObjects = value;
                OnPropertyChanged(nameof(CurrentObjects));
            }
        }

        public ObservableCollection<string> LogStrings { get; }

        private string _selectedItem = string.Empty;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion

        private ProcessStateStore _processStateStore;
        private Dispatcher _dispatcher;

        public LogViewModel(ProcessStateStore processStateStore)
        {
            _processStateStore = processStateStore;
            _dispatcher = Application.Current.Dispatcher;

            LogStrings = new ObservableCollection<string>();
            LogStrings.CollectionChanged += OnLogStringChanged;

            _processStateStore.LoadingStarted += OnLoadingStarted;
            _processStateStore.SourceLoaded += OnSourceLoaded;
            _processStateStore.LoadingCompleted += OnLoadingCompleted;

            _processStateStore.ParsingStarted += OnParsingStarted;
            _processStateStore.SourceParsed += OnSourceParsed;
            _processStateStore.ParsingCompleted += OnParsingCompleted;
        }


        #region Event handlers

        private void OnLogStringChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedItem = e.NewItems?[0] as string ?? string.Empty;
        }

        public void OnLoadingStarted(int sourcesCount)
        {
            ProcessInfo = "Loading";
            CurrentObjects = 0;
            MaxObjects = sourcesCount;

            _dispatcher.BeginInvoke(() =>
                LogStrings.Add("Loading started")
            );
        }

        internal void OnSourceLoaded(NewsSources? value, bool success)
        {
            CurrentObjects++;

            var logString = success ? "Loaded: " : "Error: ";
            logString += value.Name.ToString();

            _dispatcher.BeginInvoke(()=>
                LogStrings.Add(logString)
            );
        }

        internal void OnLoadingCompleted(int sourcesCount)
        {
            ProcessInfo = "Completed";
            CurrentObjects = sourcesCount;

            _dispatcher.BeginInvoke(() =>
                LogStrings.Add("Loading completed")
            );
        }

        internal void OnParsingStarted(int sourcesCount)
        {
            ProcessInfo = "Parsing";
            CurrentObjects = 0;
            MaxObjects = sourcesCount;

            _dispatcher.BeginInvoke(() =>
                LogStrings.Add("Parsing started")
            );
        }

        internal void OnSourceParsed(NewsSources? value, bool success)
        {
            CurrentObjects++;

            var logString = success ? "Parsed: " : "Error: ";
            logString += value.Name.ToString();

            _dispatcher.BeginInvoke(() =>
                LogStrings.Add(logString)
            );
        }

        internal void OnParsingCompleted(int sourcesCount, int entitiesCount)
        {
            ProcessInfo = "Completed";
            CurrentObjects = sourcesCount;
        }

        #endregion
    }
}
