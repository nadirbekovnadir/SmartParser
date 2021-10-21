using Models;
using ParserApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StartFindCommand : AsyncBaseCommand
    {
        private readonly NewsFinder _dataFinder;

        public StartFindCommand(NewsFinder dataFinder, Action<object> p) : base(p)
        {
            _dataFinder = dataFinder;
        }

        protected async override Task ExecuteAsync(object? parameter)
        {

        }
    }
}
