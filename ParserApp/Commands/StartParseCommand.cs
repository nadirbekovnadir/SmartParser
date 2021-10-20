using Models;
using Models.Entities;
using Models.Repositories;
using ParserApp.BindingParams;
using System;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StartParseCommand : AsyncBaseCommand
    {
        private readonly PathesParams _pathes;
        private readonly ParseParams _parse;

        private readonly DataExtractor _dataExtractor;
        private readonly IRepository<NewsBlock> _repo;

        public StartParseCommand(
            PathesParams pathes,
            ParseParams parse,
            DataExtractor dataExtractor,
            IRepository<NewsBlock> repo,
            Action<Exception> onException)
            : base(onException)
        {
            _pathes = pathes;
            _parse = parse;

            _dataExtractor = dataExtractor;
            _repo = repo;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            await _dataExtractor.StartAsync(
                _pathes.SitesFile,
                _parse.Timeout, _parse.WithRBC);

            _repo.Add(_dataExtractor.NewsBlock);
        }
    }
}
