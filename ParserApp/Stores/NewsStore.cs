using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserApp.Stores
{
    public class NewsStore
    {
        public event Action NewChanged;
        public event Action FindedChanged;

        private List<NewsEntity> _parsedAll = new List<NewsEntity>();
        public List<NewsEntity> ParsedAll
        {
            get => _parsedAll;
            set
            {
                _parsedAll = value;
                OnParsedChanged();
            }
        }

        private List<NewsEntity> _parsedNew = new List<NewsEntity>();
        public List<NewsEntity> ParsedNew
        {
            get => _parsedNew;
            set
            {
                _parsedNew = value;
                OnParsedChanged();
            }
        }

        private void OnParsedChanged()
        {
            FindedChanged?.Invoke();
        }
    }
}
