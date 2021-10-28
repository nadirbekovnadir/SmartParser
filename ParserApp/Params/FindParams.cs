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
                OnPropertyChanged(nameof(SaveNew));
            }
        }

        private bool _saveAll = true;
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
