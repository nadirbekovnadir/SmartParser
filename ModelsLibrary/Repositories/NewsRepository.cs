using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositories
{
    public class NewsRepository : IRepository<NewsEntity>
    {
        private readonly IContext _context;

        public NewsRepository(IContext context)
        {
            _context = context;
        }

        public void Add(List<NewsEntity> entity)
        {
            // Тут не так просто
            //NewsEntity.SaveToCsv(entity, _context.Path, "");
        }

        public void Add(NewsEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(NewsEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(List<NewsEntity> entities)
        {
            throw new NotImplementedException();
        }

        public List<NewsEntity> GetAll()
        {
            throw new NotImplementedException();
        }

    }
}
