using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ParserApp.Interfaces;
using ParserApp.Commands;
using ParserApp.BindingParams;

namespace ParserApp.VM
{

    public class MainWindowVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


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

        #endregion


        public MainWindowVM(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }


        #region Commands

        #region OpenSitesFile

        private RelayCommand<object>  _openSitesFile;
        public RelayCommand<object> OpenSitesFile 
        { 
            get
            {
                _openSitesFile ??= new RelayCommand<object>(OpenSitesFileExecute);
                return _openSitesFile;
            }
        }

        public void OpenSitesFileExecute(object parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            Pathes.SitesFile = _dialogService.FilePath;
        }

        #endregion

        #region OpenWordsFile

        private RelayCommand<object> _openWordsFile;
        public RelayCommand<object> OpenWordsFile
        {
            get
            {
                _openWordsFile ??= new RelayCommand<object>(OpenWordsFileExecute);
                return _openWordsFile;
            }
        }

        public void OpenWordsFileExecute(object parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            Pathes.WordsFile = _dialogService.FilePath;
        }

        #endregion

        #region OpenOutputDirectory

        private RelayCommand<object> _openOutputDirectory;
        public RelayCommand<object> OpenOutputDirectory
        {
            get
            {
                _openOutputDirectory ??= new RelayCommand<object>(OpenOutputDirectoryExecute);
                return _openOutputDirectory;
            }
        }
        private void OpenOutputDirectoryExecute(object obj)
        {
            if (!_dialogService.OpenFolderDialog())
                return;

            Pathes.Output = _dialogService.FolderPath;
        }

        #endregion

        #region StartParse

        private RelayCommand<object> _startParse;
        public RelayCommand<object> StartParse
        {
            get
            {
                _startParse ??= new RelayCommand<object>(StartParseExecute);
                return _startParse;
            }
        }
        private void StartParseExecute(object obj)
        {
            // Temp
            _dialogService.ShowMessage(Parse.Timeout.ToString());
        }

        #endregion

        #endregion
    }
}
