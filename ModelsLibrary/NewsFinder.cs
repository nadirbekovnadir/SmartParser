using Models.Entities;
using System.Text.RegularExpressions;

namespace Models
{
    public class NewsFinder
    {
        private readonly IParserService _parserService;

        public List<NewsEntity> News { get; set; } = new List<NewsEntity>();

        public NewsFinder(IParserService parserService)
        {
            _parserService = parserService;
        }

        public event EventHandler<CompletedEventArgs>? ProcessCompleted;

        public Task<int> StartAsync(List<NewsEntity> entities, string pattern)
        {
            var task = Task.Factory.StartNew(() => Start(entities, pattern));
            return task;
        }

        public int Start(List<NewsEntity> entities, string pattern)
        {
            int result = 0;
            try
            {
                var patterns = _parserService.Decompose(pattern);

                var regexes = (from p in patterns
                               select new Regex(p, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                               .ToList();

                var res = from entity in entities.AsParallel()
                          where _parserService.Compute(
                              (from r in regexes
                               select r.IsMatch(entity.Title) || r.IsMatch(entity.Description))
                               .ToList())
                          select entity;

                News = res.ToList();
            }
            catch (Exception ex)
            {
                result = -1;
            }
            finally
            {
                // Вызовется при окончании основного процесса, то есть после установки всех свойств уж точно
                ProcessCompleted?.Invoke(
                    this, new CompletedEventArgs { ExitCode = result });
            }

            return result;
        }
    }
}
