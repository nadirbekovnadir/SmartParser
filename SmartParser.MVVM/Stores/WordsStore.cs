using SmartParser.MVVM.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartParser.MVVM.Stores
{
	public class WordsStore
    {
        private readonly IWordsProvider _provider;

        public List<string> Words { get; private set; }

        public event Action Updated;

        public WordsStore(IWordsProvider provider)
        {
            Words = new List<string>();
            _provider = provider;
        }

        public void Load(string path)
        {
            Words = _provider.Load(path);
            OnUpdated();
        }

        public void Update(string value)
        {
            Words = _provider.Convert(value);
            OnUpdated();
        }

        private void OnUpdated()
        {
            Updated?.Invoke();
        }
    }
}
