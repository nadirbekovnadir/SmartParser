using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ParserApp.Interfaces;
using ParserApp.Commands;

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

        #region Binded properties

        private string _sitesFilePath;
        public string SitesFilePath
        {
            get { return _sitesFilePath; }
            set
            {
                _sitesFilePath = value;
                OnPropertyChanged(nameof(SitesFilePath));
            }
        }


        private string _wordsFilePath;

        public string WordsFilePath
        {
            get { return _wordsFilePath; }
            set
            {
                _wordsFilePath = value;
                OnPropertyChanged(nameof(WordsFilePath));
            }
        }

        #endregion

        #region Services

        private readonly IDialogService _dialogService;

        #endregion

        #region Commands

        private RelayCommand<object> _openSitesFile;
        public RelayCommand<object> OpenSitesFile
        {
            get => _openSitesFile ?? new RelayCommand<object>(OpenSitesFileExecute);
        }

        private RelayCommand<object> _openWordsFile;
        public RelayCommand<object> OpenWordsFile
        {
            get => _openWordsFile ?? new RelayCommand<object>(OpenWordsFileExecute);
        }

        #endregion

        public MainWindowVM(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }


        #region OpenSitesFile

        public void OpenSitesFileExecute(object parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            SitesFilePath = _dialogService.FilePath;
        }

        public void OpenWordsFileExecute(object parameter)
        {
            if (!_dialogService.OpenFileDialog())
                return;

            WordsFilePath = _dialogService.FilePath;
        }

        #endregion
    }
}
