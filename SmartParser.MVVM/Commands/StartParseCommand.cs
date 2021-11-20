using Microsoft.Extensions.Logging;
using SmartParser.Database.Repositories.Common;
using SmartParser.Domain.Entities;
using SmartParser.Domain.Services.Common;
using SmartParser.MVVM.Commands.Common;
using SmartParser.MVVM.Stores;
using SmartParser.MVVM.ViewModels;
using SmartParser.MVVM.ViewModels.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmartParser.MVVM.Commands
{
	public class StartParseCommand : AsyncBaseCommand
    {
        private readonly PathesParams _pathes;
        private readonly ParseParams _parse;
        private readonly NewsStore _newsStore;
        private readonly ILogger _logger;
        private readonly INewsExtractor _dataExtractor;
        private readonly IRepository<NewsEntity> _repo;

        public StartParseCommand(
            ProcessesViewModel vm,
            ILogger logger,
            NewsStore newsStore,
            INewsExtractor dataExtractor,
            IRepository<NewsEntity> repo)
        {
            _pathes = vm.Pathes;
            _parse = vm.Parse;
            _newsStore = newsStore;

            _logger = logger;

            _dataExtractor = dataExtractor;
            _repo = repo;
        }

        protected override async Task Execution(object? parameter)
        {
            _logger.LogInformation("Parsing started");

            await _dataExtractor.StartAsync(
                _pathes.SitesFile,
                _parse.Timeout, _parse.WithRBC);

            _repo.Add(_dataExtractor.News);

            _newsStore.ParsedNew = NewsEntity.Except(_dataExtractor.News, _newsStore.ParsedAll);
            _newsStore.ParsedAll = _dataExtractor.News;

            _logger.LogInformation(
                $"News extracted: all - {_newsStore.ParsedAll.Count}, new - {_newsStore.ParsedNew.Count}");

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dir = Path.Combine(_pathes.Output, "Parsed");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var sheetes = new List<string>();
            var entities = new List<List<NewsEntity>>(); 

            if (_parse.SaveAll && _newsStore.ParsedAll.Count != 0)
            {
                sheetes.Add("All");
                entities.Add(_newsStore.ParsedAll.ConvertAll(news => new NewsEntity(news)));
            }


            if (_parse.SaveNew && _newsStore.ParsedNew.Count != 0)
            {
                sheetes.Add("New");
                entities.Add(_newsStore.ParsedNew.ConvertAll(news => new NewsEntity(news)));
            }

            NewsEntity.SaveToExcel(entities, dir, timestamp, sheetes);
            _logger.LogInformation($"Parsed news saved to: {Path.Join(dir, timestamp)}");

            _logger.LogInformation("Parsing ended");
        }
    }
}
