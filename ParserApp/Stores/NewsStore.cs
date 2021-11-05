using SmartParser.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SmartParser.MVVM.Stores
{
	public class NewsStore
    {
        public event Action FindedChanged;
        public event Action ParsedChanged;

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


        private List<List<NewsEntity>> _findedAll = new List<List<NewsEntity>>();
        public List<List<NewsEntity>> FindedAll
        {
            get => _findedAll;
            set
            {
                _findedAll = value;
                OnFindedChanged();
            }
        }

        private List<List<NewsEntity>> _findedNew = new List<List<NewsEntity>>();
        public List<List<NewsEntity>> FindedNew
        {
            get => _findedNew;
            set
            {
                _findedNew = value;
                OnFindedChanged();
            }
        }

        private void OnParsedChanged()
        {
            ParsedChanged?.Invoke();
        }

        private void OnFindedChanged()
        {
            FindedChanged?.Invoke();
        }
    }
}
