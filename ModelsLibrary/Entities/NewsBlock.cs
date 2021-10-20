using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class NewsBlock
    {
        private List<NewsEntity> _entities;
        public List<NewsEntity> Entities { get => _entities; set => _entities = value; }

        public NewsBlock(List<NewsEntity> entities)
        {
            this.Entities = entities;
        }

        public NewsEntity this[int key]
        {
            get => Entities[key];
            set => Entities[key] = value;
        }


        public static implicit operator NewsBlock(List<NewsEntity> entities)
            => new NewsBlock(entities);

        public static NewsBlock LoadFromCsv(string path)
        {
            return NewsEntity.LoadFromCsv(path);
        }
        public static void SaveToCsv(NewsBlock block, string path)
        {
            NewsEntity.SaveToCsv(block.Entities, path);
        }
    }
}
