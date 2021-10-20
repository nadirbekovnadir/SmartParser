using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class CompletedEventArgs : EventArgs
    {
        public int ExitCode { get; set; }
    }
}
