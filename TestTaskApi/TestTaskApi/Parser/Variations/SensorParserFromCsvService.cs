using System.Globalization;
using TestTaskApi.Models.Data;

namespace TestTaskApi.Parser.Variations
{
    //new parser, simple and faster
    public class SensorParserFromCsvService : ISensorParserFromCsvService
    {
        
        private const int _timestamp = 0;
        private const int _sensorName = 1;
        private const int _customerName = 2;
        private const int _flags = 3;
        private const int _sensorType = 4;
        private const int _unit = 5;
        private const int _east = 6;
        private const int _north = 7;
        private const int _height = 8;
        private const int _km = 9;
        private const int _value1 = 10;
        private const int _value2 = 11;

        private const char separator = ';';

        public Task<IEnumerable<Sensor>> ParseAsync(IFormFile file)
            => Task.Run(() => Parse(file));

        public IEnumerable<Sensor> Parse(IFormFile file)
        {
            var sensors = new Dictionary<string, Sensor>();
            using (var reader = new StreamReader(file.OpenReadStream())) //read file line by line
            {
                reader.ReadLine(); //skip header line
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    ParseSensor(line, sensors);
                }
            }
            return sensors.Values.ToList();
        }
        
        private void ParseSensor(string line, Dictionary<string, Sensor> sensors)
        {
            var lineValues = line.Split(separator);
            if (lineValues.Length < 12)
                return;

            var sensorName = lineValues[_sensorName];
                   
            var sensorValue = new SensorValues 
            { 
                SensorName = sensorName,
                DateTime = ToTicks(lineValues[_timestamp]),
                Values1 = ToDouble(lineValues[_value1]),
                Values2 = ToDouble(lineValues[_value2]),
            };
            

            if(sensors.TryGetValue(sensorName, out var sensor)) //if sensor has been inserted in dictionary then just add new values
            {
                sensor.SensorValues.Add(sensorValue);
                return;
            }
        
            sensors.Add(sensorName, new Sensor //else add new sensor in dictionary
            {
                SensorName = sensorName,
                CustomerName = lineValues[_customerName],
                Flags = lineValues[_flags],
                SensorType = lineValues[_sensorType],
                Unit = lineValues[_unit],
                East = ToDouble(lineValues[_east]),
                North = ToDouble(lineValues[_north]),
                Km = ToDouble(lineValues[_km]),
                Height = ToDouble(lineValues[_height]),
                SensorValues = new List<SensorValues> { sensorValue }
            });
            
        }
        private long ToTicks(string date)
        {
            if (DateTime.TryParse(date, out var dateTime))
            {
                return dateTime.Ticks;
            }
            throw new FormatException($"Date format is incorrect: {date}");
        }
        private double ToDouble(string value) 
            => Convert.ToDouble(value , CultureInfo.InvariantCulture);
    }
}
