using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Models.Repository.Variations.SensorValueRepository;
using TestTaskApi.Models.Transfer;
using TestTaskApi.Parser.Variations;


namespace TestTaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _sensorRepository; 
        private readonly ISensorValueRepository _sensorValueRepository;
        private readonly ISensorParserFromCsvService _parser;
       

        private static object _lockObject = new object();

        public SensorController(ISensorRepository sensorRepository, ISensorValueRepository sensorValueRepository, ISensorParserFromCsvService parser)
        {
            _sensorRepository = sensorRepository;
            _sensorValueRepository = sensorValueRepository;
            _parser = parser;
        
        }

        [HttpPost("UploadSensorData")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> LoadSensorData(IFormFile file)
        {
            if (file == null || file.Length == 0) 
            {
                return BadRequest("Empty file uploaded.");
            }

            IEnumerable<Sensor> sensors;
            try
            {
                sensors = await _parser.ParseAsync(file);              
            }
            catch (Exception ex)
            {
                return BadRequest($"Error parsing sensor data.");
            }

            var sensorsValues = sensors.SelectMany(sensor => sensor.SensorValues);
            lock (_lockObject) //use a lock for case that file has not yet been updated in db, but next file has already been uploaded
            {
                try
                {
                    _sensorRepository.AddMany(sensors);
                    _sensorValueRepository.AddMany(sensorsValues);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error saving sensor data.");
                }
            }


            return Created();
        }       
        [HttpGet("SensorsLoacation")]
        public async Task<IEnumerable<SensorLocationMinDto>> GetAllSensors()
        {
            var sensorsDto = new List<SensorLocationMinDto>();
            var sensors = await _sensorRepository.GetAllAsync();
            foreach (var sensor in sensors) 
            {
                var sensorName = sensor.SensorName;
                var latestValue = await _sensorValueRepository.GetValuesFromLastDateAsync(sensorName);
                var valuesCountBySensor = await _sensorValueRepository.GetAllValuesCountAsync(sensorName); 

                sensorsDto.Add(new SensorLocationMinDto
                {
                    Sensor_name = sensorName,
                    East = sensor.East,
                    North = sensor.North,
                    Value1 = latestValue.Values1,
                    Value2 = latestValue.Values2,
                    Value_count = valuesCountBySensor
                });
            }
            return sensorsDto;
        }

    }
}
