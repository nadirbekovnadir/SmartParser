using Models.Entities;

namespace Models
{
    public interface INewsFinder
    {
        List<NewsEntity> News { get; set; }

        event EventHandler<CompletedEventArgs>? ProcessCompleted;

        /// <summary>
        /// Starts the process of news finding
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="pattern"></param>
        /// <returns>Code of process</returns>
        /// <exception cref="Exception">Thrown if process fails</exception>
        int Start(List<NewsEntity> entities, string pattern);

        /// <summary>
        /// Starts the process of news finding asynchroniously
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="pattern"></param>
        /// <returns>Code of process</returns>
        /// <exception cref="Exception">Thrown if process fails</exception>
        Task<int> StartAsync(List<NewsEntity> entities, string pattern);
    }
}