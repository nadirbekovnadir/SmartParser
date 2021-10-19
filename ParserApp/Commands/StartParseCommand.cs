using Models;
using ParserApp.BindingParams;
using ParserApp.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StartParseCommand : BaseCommand
    {
        private readonly ParseParams _parseParams;
        private readonly PathesParams _pathesParams;
        private readonly DataExtractor _dataExtractor;

        public StartParseCommand(ParseParams parseParams, PathesParams pathesParams, DataExtractor dataExtractor)
        {
            _parseParams = parseParams;
            _pathesParams = pathesParams;
            _dataExtractor = dataExtractor;
        }

        public override void Execute(object? parameter)
        {
            _dataExtractor.StartAsync(
                _pathesParams.SitesFile, _pathesParams.Output,
                _parseParams.Timeout, _parseParams.WithRBC);
        }
    }
}
