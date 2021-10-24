using Ganss.Excel;
using Models;
using Models.Entities;
using Models.Repositories;
using ParserApp.BindingParams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ParserApp.Commands
{
    public class StartParseCommand : AsyncBaseCommand
    {
        private readonly PathesParams _pathes;
        private readonly ParseParams _parse;

        private readonly NewsExtractor _dataExtractor;
        private readonly IRepository<NewsEntity> _repo;

        public StartParseCommand(
            PathesParams pathes,
            ParseParams parse,
            NewsExtractor dataExtractor,
            IRepository<NewsEntity> repo,
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

            _repo.Add(_dataExtractor.News);

            var excepted = NewsEntity.Except(_dataExtractor.News, new List<NewsEntity>());

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dir = Path.Combine(_pathes.Output, "Parsed");

            var sheetes = new List<string>();
            var entities = new List<List<NewsEntity>>(); 

            if (_parse.SaveAll)
            {
                sheetes.Add("All");
                entities.Add(_dataExtractor.News);
            }


            if (_parse.SaveNew)
            {
                sheetes.Add("New");
                entities.Add(excepted);
            }

            NewsEntity.SaveToExcel(entities, dir, timestamp, sheetes);
        }
    }
}
