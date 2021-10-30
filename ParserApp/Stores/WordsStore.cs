using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParserApp.Services;

namespace ParserApp.Stores
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
