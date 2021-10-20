using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.BindingParams
{
    public class FindParams : BaseParams
    {
        private bool _withAll = true;
        public bool WithAll
        {
            get => _withAll;
            set
            {
                _withAll = value;
                OnPropertyChanged(nameof(WithAll));
            }
        }

        private bool _withExcepted = false;
        public bool WithExcepted
        {
            get => _withExcepted;
            set
            {
                _withExcepted = value;
                OnPropertyChanged(nameof(WithExcepted));
            }
        }

        private bool _toExcel = true;
        public bool ToExcel
        {
            get => _toExcel;
            set
            {
                _toExcel = value;
                OnPropertyChanged(nameof(ToExcel));
            }
        }
    }
}
