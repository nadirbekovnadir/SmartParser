using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Params
{
    public class FindParams : BaseParams
    {
        private bool _saveNew = true;
        public bool SaveNew
        {
            get => _saveNew;
            set
            {
                _saveNew = value;

                if (!_saveNew && AccumulateNew)
                    AccumulateNew = false;

                OnPropertyChanged(nameof(SaveNew));
            }
        }

        private bool _accumulateNew = false;
        public bool AccumulateNew
        {
            get => _accumulateNew;
            set
            {
                _accumulateNew = value;
                OnPropertyChanged(nameof(AccumulateNew));
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
