using SmartParser.Domain.Entities;
using System;

namespace SmartParser.MVVM.Stores
{
	public class ProcessStateStore
    {
        public event Action<int> LoadingStarted;
        public event Action<NewsSources?, bool> SourceLoaded;
        public event Action<int> LoadingCompleted;

        public event Action<int> ParsingStarted;
        public event Action<NewsSources?, bool> SourceParsed;
        public event Action<int, int> ParsingCompleted;

        public void OnLoadingStarted(int sourcesCount)
        {
            LoadingStarted?.Invoke(sourcesCount);
        }

        internal void OnSourceLoaded(NewsSources? value, bool success)
        {
            SourceLoaded?.Invoke(value, success);
        }

        internal void OnLoadingCompleted(int sourcesCount)
        {
            LoadingCompleted?.Invoke(sourcesCount);
        }

        internal void OnParsingStarted(int sourcesCount)
        {
            ParsingStarted?.Invoke(sourcesCount);
        }

        internal void OnSourceParsed(NewsSources? value, bool success)
        {
            SourceParsed?.Invoke(value, success);
        }

        internal void OnParsingCompleted(int sourcesCount, int entitiesCount)
        {
            ParsingCompleted?.Invoke(sourcesCount, entitiesCount);
        }
    }
}
