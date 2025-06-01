namespace TestTaskApi.Models.Repository.Base
{
    public interface IRepository<T,TId>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TId id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteByIdAsync(TId id);
       
        Task ClearAsync();
    }
}
