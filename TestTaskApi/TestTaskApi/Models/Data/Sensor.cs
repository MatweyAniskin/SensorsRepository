using MongoDB.Bson.Serialization.Attributes;
using TestTaskApi.Models.Repository.Reflection;

namespace TestTaskApi.Models.Data
{
    [CollectionName("SensorsCollection")] 
    public class Sensor
    {
        [BsonId]
        [BsonElement("sensor_name")]        
        public string SensorName { get; set; }

        [BsonElement("customer_name")]
        public string CustomerName { get; set; }

        [BsonElement("flags")]
        public string Flags { get; set; }

        [BsonElement("sensor_type")]
        public string SensorType { get; set; }

        [BsonElement("unit")]
     
        public string Unit { get; set; }

        [BsonElement("north")]
        public double North { get; set; }

        [BsonElement("east")]
        public double East { get; set; }

        [BsonElement("height")]
        public double Height { get; set; }

        [BsonElement("km")]    
        public double Km { get; set; }
        [BsonIgnore]
        public List<SensorValues> SensorValues { get; set; } = new List<SensorValues>(); //using only in parser
        
    }
}
