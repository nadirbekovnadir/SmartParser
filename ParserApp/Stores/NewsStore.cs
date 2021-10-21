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


        private List<NewsEntity> _new;
        public List<NewsEntity> New
        {
            get => _new;
            set
            {
                _new = value;
                OnNewChanged();
            }
        }

        private List<NewsEntity> _finded;
        public List<NewsEntity> Finded
        {
            get => _finded;
            set
            {
                _finded = value;
                OnFindedChanged();
            }
        }

        private void OnNewChanged()
        {
            NewChanged?.Invoke();
        }

        private void OnFindedChanged()
        {
            FindedChanged?.Invoke();
        }
    }
}
