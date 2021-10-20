using Models;
using ParserApp.BindingParams;
using ParserApp.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StartParseCommand : AsyncBaseCommand
    {
        private readonly ProcessesViewModel _viewModel;
        private readonly DataExtractor _dataExtractor;

        public StartParseCommand(
            ProcessesViewModel viewModel, 
            DataExtractor dataExtractor,
            Action<Exception> onException)
            : base(onException)
        {
            _viewModel = viewModel;
            _dataExtractor = dataExtractor;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            var outputPath = Path.Combine(_viewModel.Pathes.Output, _viewModel.ParseFolder);

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            await _dataExtractor.StartAsync(
                _viewModel.Pathes.SitesFile, outputPath,
                _viewModel.Parse.Timeout, _viewModel.Parse.WithRBC);
        }
    }
}
