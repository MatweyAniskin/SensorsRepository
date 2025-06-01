using MongoDB.Bson.Serialization.Attributes;
using TestTaskApi.Models.Repository.Reflection;
using TestTaskApi.Parser.Reflection;

namespace TestTaskApi.Models.Data
{
    [CollectionName("SensorsValuesCollection")] //атрибут для название коллекции в монго
    public class Sensor //модель данных датчика
    {
        [BsonId]
        [BsonElement("sensor_name")]
        [ParseProperty] //мои атрибуты для указания какие поля нужно парсить, по хорошему надо было сделать вторую модель, но так наглядней
        public string SensorName { get; set; }

        [BsonElement("date_time")]
        [ParseProperty]
        public string[] DateTime { get; set; }

        [BsonElement("customer_name")]
        [ParseProperty]
        public string CustomerName { get; set; }

        [BsonElement("flags")]
        [ParseProperty]
        public string Flags { get; set; }

        [BsonElement("sensor_type")]
        [ParseProperty]
        public string SensorType { get; set; }

        [BsonElement("unit")]
        [ParseProperty]
        public string Unit { get; set; }

        [BsonElement("north")]
        [ParseProperty]
        public double North { get; set; }

        [BsonElement("east")]
        [ParseProperty]
        public double East { get; set; }

        [BsonElement("height")]
        [ParseProperty]
        public double Height { get; set; }

        [BsonElement("km")]
        [ParseProperty("km")]
        public double Km { get; set; }

        [BsonElement("value1")]
        [ParseProperty("VALUE1")] //так же указываю название заголовков с файла, если они отличаются с названием поля
        public double[] Values1 { get; set; }

        [BsonElement("value2")]
        [ParseProperty("VALUE2")]
        public double[] Values2 { get; set; }
    }
}
