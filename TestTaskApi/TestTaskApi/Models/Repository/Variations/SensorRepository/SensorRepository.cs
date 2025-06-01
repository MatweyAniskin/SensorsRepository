using TestTaskApi.DataBase;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorRepository
{
    //Репозиторий датчиков, наследуется от обобщенного репозитория
    public class SensorRepository : BaseRepository<Sensor, string>, ISensorRepository
    {
        public SensorRepository(IMongoDb mongoDb) : base(mongoDb)
        {
        }
    }
}
