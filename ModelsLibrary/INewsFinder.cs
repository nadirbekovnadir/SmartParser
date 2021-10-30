using Models.Entities;

namespace Models
{
    public interface INewsFinder
    {
        List<NewsEntity> News { get; set; }

        event EventHandler<CompletedEventArgs>? ProcessCompleted;

        int Start(List<NewsEntity> entities, string pattern);
        Task<int> StartAsync(List<NewsEntity> entities, string pattern);
    }
}