using SmartParser.Database.Contexts.Common;
using SmartParser.Database.Repositories.Common;
using SmartParser.Domain.Entities;


namespace SmartParser.Database.Repositories
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
