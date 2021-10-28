using ParserApp.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Params
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

    }
}
