namespace TestTaskApi.Models.Transfer
{
    public class SensorLocationMinDto //Дто датчика
    {
        public string Sensor_name {  get; set; }
        public double North {  get; set; }
        public double East { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public long Value_count {  get; set; }
    }
}
