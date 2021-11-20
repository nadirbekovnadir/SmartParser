using SmartParser.MVVM.ViewModels.Parameters.Common;
using System;
using System.ComponentModel;

namespace SmartParser.MVVM.ViewModels.Parameters
{
	public class AutoParams : BaseParams
    {
        private int _delayMinutes = 15;

        public int DelayMinutes
        {
            get { return _delayMinutes; }
            set 
            { 
                _delayMinutes = value; 
                OnPropertyChanged(nameof(DelayMinutes));
            }
        }

        private bool _mustParse;
        public bool MustParse
        {
            get => _mustParse;
            set
            {
                _mustParse = value;
                OnPropertyChanged(nameof(MustParse));
            }
        }

        private bool _mustFind;
        public bool MustFind
        {
            get => _mustFind;
            set
            {
                _mustFind = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

		public Action<object?, PropertyChangedEventArgs> PropertyChanged { get; internal set; }
	}
}
