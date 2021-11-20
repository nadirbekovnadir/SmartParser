using SmartParser.MVVM.ViewModels.Parameters.Common;

namespace SmartParser.MVVM.ViewModels.Parameters
{
	public class ParseParams : BaseParams
    {
        private bool _withRBC = false;
        public bool WithRBC 
        { 
            get => _withRBC; 
            set
            {
                _withRBC = value;
                OnPropertyChanged(nameof(WithRBC));
            }
        }

        private int _timeout = 20;
        public int Timeout
        {
            get => _timeout;
            set
            {
                _timeout = value;
                OnPropertyChanged(nameof(Timeout));
            }
        }

        private bool _saveNew = true;
        public bool SaveNew
        {
            get => _saveNew;
            set
            {
                _saveNew = value;
                OnPropertyChanged(nameof(SaveNew));
            }
        }

        private bool _saveAll = false;
        public bool SaveAll
        {
            get => _saveAll;
            set
            {
                _saveAll = value;
                OnPropertyChanged(nameof(SaveAll));
            }
        }
    }
}
