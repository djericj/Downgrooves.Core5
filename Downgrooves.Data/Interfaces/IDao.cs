namespace Downgrooves.Data.Interfaces
{
    public interface IDao<out T> where T : class
    {
        IQueryable<T> GetData(string filePath);

        IEnumerable<T?> GetAll();

        T? Get(int id);

        T? Get(string name);
    }
}
