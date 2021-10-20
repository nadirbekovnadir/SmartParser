using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositories
{
    public class NewsRepository : IRepository<NewsBlock>
    {
        private readonly IContext _context;

        public NewsRepository(IContext context)
        {
            _context = context;
        }

        public void Add(NewsBlock entity)
        {
            // Тут не так просто
            NewsBlock.SaveToCsv(entity, _context.Path);
        }

        public NewsBlock Get(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NewsBlock> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllNames()
        {
            throw new NotImplementedException();
        }

        public NewsBlock GetLast()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NewsBlock> GetLast(int depth)
        {
            throw new NotImplementedException();
        }

        public void Remove(NewsBlock entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(string name)
        {
            throw new NotImplementedException();
        }
    }
}
