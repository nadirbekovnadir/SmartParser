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

        /// <summary>
        /// Starts the process of news exctracting
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="timeout"></param>
        /// <param name="with_rbk"></param>
        /// <returns>Code of process</returns>
        /// <exception cref="Exception">Thrown if process fails</exception>
        int Start(string inputPath, int timeout = 15, bool with_rbk = false);
        
        /// <summary>
        /// Starts the process of news exctracting asynchroniously
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="timeout"></param>
        /// <param name="with_rbk"></param>
        /// <returns>Code of process</returns>
        /// <exception cref="Exception">Thrown if process fails</exception>
        Task<int> StartAsync(string inputPath, int timeout = 15, bool with_rbk = false);
    }
}