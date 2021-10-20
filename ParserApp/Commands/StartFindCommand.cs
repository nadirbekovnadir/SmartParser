using Models;
using ParserApp.VM;
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
        private readonly ProcessesViewModel _viewModel;
        private readonly DataFinder _dataFinder;

        public StartFindCommand(ProcessesViewModel processesViewModel, DataFinder dataFinder, Action<object> p) : base(p)
        {
            _viewModel = processesViewModel;
            _dataFinder = dataFinder;
        }

        protected async override Task ExecuteAsync(object? parameter)
        {
            List<string> inputPathes = new List<string>();

            var outputPath = Path.Combine(_viewModel.Pathes.Output, _viewModel.FindFolder);
            List<string> outputPathes = new List<string>();

            if (_viewModel.Find.WithAll)
            {
                AddPathes(_viewModel.ParseFolder);
            }

            if (_viewModel.Find.WithExcepted)
            {
                AddPathes(_viewModel.ExceptFolder);
            }

            for (int i = 0; i < inputPathes.Count; ++i)
            {
                await _dataFinder.StartAsync(
                    _viewModel.Pathes.WordsFile, 
                    inputPathes[i], outputPathes[i]);
            }

            return;

            void AddPathes(string folder)
            {
                var path = Path.Combine(
                        _viewModel.Pathes.Output,
                        folder);
                if (!Directory.Exists(path))
                    return;

                inputPathes.Add(path);
                outputPathes.Add(outputPath + "_" + folder);
            }
        }
    }
}
