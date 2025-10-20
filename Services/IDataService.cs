namespace MauiApp3
{
    // Generic interface for data service operations
    public interface IDataService<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}