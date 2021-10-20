using Models.Entities;

namespace Models
{
    public class DataFinder
    {
        public DataFinder()
        {

        }

        public List<NewsEntity> NewsEntities {  get; set; }

        public event EventHandler<CompletedEventArgs>? ProcessCompleted;

        public Task<int> StartAsync(string wordsPath, string filePath, string outputPath)
        {
            var task = Task.Factory.StartNew(() => Start(wordsPath, filePath, outputPath));
            task.ContinueWith(
                (a) => 
                    ProcessCompleted?.Invoke(
                    this, new CompletedEventArgs { ExitCode = a.Result }
                )
            );

            return task;
        }

        private int Start(string wordsPath, string filePath, string outputPath)
        {
            try
            {
                var patterns = File.ReadAllLines(wordsPath);
                var entities = NewsEntity.LoadFromCsv(filePath);

                NewsEntities = DataHelper.Match(entities, patterns);
                NewsEntity.SaveToCsv(NewsEntities, outputPath);
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;
        }
    }
}
