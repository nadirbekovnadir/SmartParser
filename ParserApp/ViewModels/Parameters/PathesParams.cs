using SmartParser.MVVM.ViewModels.Parameters.Common;

namespace SmartParser.MVVM.ViewModels.Parameters
{
	public class PathesParams : BaseParams
    {
        private string _sitesFile;
        public string SitesFile
        {
            get => _sitesFile;
            set
            {
                _sitesFile = value;
                OnPropertyChanged(nameof(SitesFile));
            }
        }

        private string _wordsFile;
        public string WordsFile
        {
            get => _wordsFile;
            set
            {
                _wordsFile = value;
                OnPropertyChanged(nameof(WordsFile));
            }
        }

        private string _output;
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

    }
}
