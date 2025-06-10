using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorValueRepository
{
    public interface ISensorValueRepository : IRepository<SensorValues>
    {
        public Task<SensorValues> GetValuesFromLastDateAsync(string sensorName);
        public Task<long> GetAllValuesCountAsync(string sensorName);
    }
}
