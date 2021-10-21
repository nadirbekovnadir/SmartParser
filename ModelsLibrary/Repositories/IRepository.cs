namespace Models.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Add(List<T> entities);

        void Remove(T entity);
        void Remove(List<T> entities);

        List<T> GetAll();
    }
}