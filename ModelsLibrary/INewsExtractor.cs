using Models.Entities;

namespace Models
{
    public interface INewsExtractor
    {
        List<NewsEntity> News { get; set; }
        NewsExtractor.ProcessState State { get; }

        event EventHandler<NewsExtractor.LoadingEventArgs>? LoadingCompleted;
        event EventHandler<NewsExtractor.LoadingEventArgs>? LoadingStarted;
        event EventHandler<NewsExtractor.ParsingEventArgs>? ParsingCompleted;
        event EventHandler<NewsExtractor.ParsingEventArgs>? ParsingStarted;
        event EventHandler<CompletedEventArgs>? ProcessCompleted;
        event EventHandler? ProcessStarted;
        event EventHandler<NewsExtractor.LoadedEventArgs>? SourceLoaded;
        event EventHandler<NewsExtractor.ParsedEventArgs>? SourceParsed;

        int Start(string inputPath, string outputPath, int timeout = 15, bool with_rbk = false);
        Task<int> StartAsync(string inputPath, int timeout = 15, bool with_rbk = false);
    }
}