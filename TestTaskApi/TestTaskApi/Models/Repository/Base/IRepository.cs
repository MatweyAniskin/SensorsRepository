namespace TestTaskApi.Models.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddMany(IEnumerable<T> entities);
        Task ClearAsync();
    }
}
