using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorRepository
{
    public interface ISensorRepository : IRepository<Sensor>
    {
        public Task<IEnumerable<Sensor>> GetAllAsync();
    }
}
