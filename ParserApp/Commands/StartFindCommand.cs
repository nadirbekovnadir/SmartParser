using Models;
using Models.Entities;
using Models.Repositories;
using ParserApp.BindingParams;
using ParserApp.Stores;
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
        private readonly PathesParams _pathes;
        private readonly FindParams _find;

        private readonly WordsStore _wordsStore;
        private readonly NewsStore _newsStore;
        private readonly NewsFinder _dataFinder;

        public StartFindCommand(
            ProcessesViewModel vm,
            WordsStore wordsStore,
            NewsStore newsStore,
            NewsFinder dataFinder,
            Action<Exception> onException)
            : base(onException)
        {
            _pathes = vm.Pathes;
            _find = vm.Find;

            _wordsStore = wordsStore;
            _newsStore = newsStore;
            _dataFinder = dataFinder;
        }

        protected async override Task ExecuteAsync(object? parameter)
        {
            List<string> patterns = new List<string>(_wordsStore.Words);

            // Можно конечно просто очистить список store а в конце вызвать метод, говорящий о том, что сущности обновились
            var findedNew = new List<List<NewsEntity>>();
            var findedAll = new List<List<NewsEntity>>();

            foreach (var pattern in patterns)
            {
                await _dataFinder.StartAsync(_newsStore.ParsedNew, pattern); // Создать store для всех слов, которые используются
                findedNew.Add(_dataFinder.News); // такое получение данных надо будет отрефакторить
                await _dataFinder.StartAsync(_newsStore.ParsedAll, pattern);
                findedAll.Add(_dataFinder.News);
            }

            _newsStore.FindedAll = findedAll;
            _newsStore.FindedNew = findedNew;
  

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string dir = Path.Combine(_pathes.Output, "Finded");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (_find.SaveAll)
            {
                var sheetes = new List<string>();
                var entities = new List<List<NewsEntity>>();

                for (int i = 0; i < _newsStore.FindedAll.Count; i++)
                {
                    sheetes.Add($"Finded_{i}");
                    entities.Add(_newsStore.FindedAll[i].ConvertAll(news => new NewsEntity(news)));
                }

                NewsEntity.SaveToExcel(entities, dir, "All_" + timestamp, sheetes);
            }

            if (_find.SaveNew)
            {
                var sheetes = new List<string>();
                var entities = new List<List<NewsEntity>>();

                for (int i = 0; i < _newsStore.FindedNew.Count; i++)
                {
                    sheetes.Add($"Finded_{i}");
                    entities.Add(_newsStore.FindedNew[i].ConvertAll(news => new NewsEntity(news)));
                }

                NewsEntity.SaveToExcel(entities, dir, "New_" + timestamp, sheetes);
            }
        }


    }
}
