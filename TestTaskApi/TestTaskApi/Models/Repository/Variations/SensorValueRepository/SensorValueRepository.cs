using MongoDB.Bson;
using MongoDB.Driver;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Base;

namespace TestTaskApi.Models.Repository.Variations.SensorValueRepository
{
    //Repository for sensors value
    public class SensorValueRepository : BaseRepository<SensorValues>, ISensorValueRepository
    {
        public SensorValueRepository(IMongoDb mongoDb) : base(mongoDb)
        {
            CreateIndex();
        }
        //Create index in db to ensure that there is no duplicate data
        protected void CreateIndex()
        {            
            var existingIndexes = _collection.Indexes.List();
           

            while (existingIndexes.MoveNext()) //If index exist in db then abort creating
            {
                foreach (var index in existingIndexes.Current)
                {
                    var indexKey = index["key"].ToJson();
                    if (indexKey.Contains("sensor_name") && indexKey.Contains("date_time"))
                    {
                        return;
                    }
                }
            }
           
            var indexKeys = Builders<SensorValues>.IndexKeys
                    .Ascending(s => s.SensorName)
                    .Ascending(s => s.DateTime);

            var indexOptions = new CreateIndexOptions { Unique = true };
            _collection.Indexes.CreateOne(new CreateIndexModel<SensorValues>(indexKeys, indexOptions));
        }

        public async Task<SensorValues> GetValuesFromLastDateAsync(string sensorName)
        {
            var filter = Builders<SensorValues>.Filter.Eq(filter => filter.SensorName, sensorName);
            return await _collection.Find(filter)
                .SortByDescending(sort => sort.DateTime)
                .FirstOrDefaultAsync();
        }

        public async Task<long> GetAllValuesCountAsync(string sensorName)
        {
            var filter = Builders<SensorValues>.Filter.Eq(item => item.SensorName, sensorName);
            return await _collection.CountDocumentsAsync(filter);
        }       
    }
}
