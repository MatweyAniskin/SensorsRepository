using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Text;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorRepository
{
    //Repository for sensors
    public class SensorRepository : BaseRepository<Sensor>, ISensorRepository
    {
        public SensorRepository(IMongoDb mongoDb) : base(mongoDb)
        {
        }

        public async Task<IEnumerable<Sensor>> GetAllAsync()
        {
            var result = await _collection.FindAsync(Builders<Sensor>.Filter.Empty);
            return await result.ToListAsync();
        }
    }
}
