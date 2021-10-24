using Models.Entities;

namespace Models
{
    public class NewsFinder
    {
        public List<NewsEntity> News { get; set; } = new List<NewsEntity>();

        public NewsFinder()
        {

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
                //something
                //Небольшой заполнитель для проверки работы
                News = new List<NewsEntity>
                {
                    new NewsEntity
                    {
                        Name = "first"
                    },
                    new NewsEntity
                    {
                        Name = "second"
                    }
                };
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
