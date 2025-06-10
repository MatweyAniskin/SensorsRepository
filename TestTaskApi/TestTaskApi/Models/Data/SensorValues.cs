using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TestTaskApi.Models.Repository.Reflection;

namespace TestTaskApi.Models.Data
{
    [CollectionName("SensorsValuesCollection")]
    public class SensorValues
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("sensor_name")]
        public string SensorName { get; set; }
        [BsonElement("date_time")]
        public long DateTime { get; set; }
        [BsonElement("value1")]        
        public double Values1 { get; set; }

        [BsonElement("value2")]        
        public double Values2 { get; set; }
    }
}
