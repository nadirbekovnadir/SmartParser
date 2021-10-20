namespace Models.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(T entity);
        void Remove(string name);

        T GetLast();
        IEnumerable<T> GetLast(int depth);
        T Get(string name);

        IEnumerable<T> GetAll();
        IEnumerable<string> GetAllNames();
    }
}