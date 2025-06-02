using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TestTaskApi.Models.Repository.Variations.SensorRepository
{
    public class SensorRecord
    {
        [BsonId]
        [BsonElement("sensor_name")]
        public string SensorName { get; set; }

        [BsonElement("timestamp_gridfs_id")]
        public ObjectId? DateTimeGridFsId { get; set; }

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

        [BsonElement("value1_gridfs_id")]
        public ObjectId? Values1GridFsId { get; set; }

        [BsonElement("value2_gridfs_id")]
        public ObjectId? Values2GridFsId { get; set; }
    }
}
