using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskApi.Models;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Models.Transfer;
using TestTaskApi.Parser.Service;

namespace TestTaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {        
        private readonly ISensorRepository _sensorRepository; //Использую репозиторий как прокси для crud операций с бд
        private readonly IParserService _parser; // Получаю сервис парсера
        private readonly IMapper _mapper; //Автомапер, чтобы мапить дто

        private static object _lockObject = new object();

        public SensorController(ISensorRepository sensorRepository, IParserService parser, IMapper mapper) 
        { 
            _sensorRepository = sensorRepository;
            _parser = parser;
            _mapper = mapper;
        }

        [HttpPost("UploadSensorData")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> LoadSensorData(IFormFile file)
        {
            if (file == null || file.Length == 0) //проверка на пустой файл
            {
                return BadRequest("Empty file uploaded.");
            }
            
            var content = string.Empty;
            using (var reader = new StreamReader(file.OpenReadStream())) //читаю данные с файла
            {
               content = await reader.ReadToEndAsync();
            }

            Sensor sensor;
            try
            {
                sensor = await _parser.ParseAsync<Sensor>(content); //использую парсинг по своей же модели
            }
            catch (Exception ex)
            {
                return BadRequest($"Error parsing sensor data.");
            }

            lock (_lockObject) //использую лок для кейса, что файл ещё не обновлен в бд, а следующий файл уже загружен
            {
                try
                {
                     _sensorRepository.AddAsync(sensor);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error saving sensor data.");
                }
            }


            return Created();
        }
        [HttpGet("SensorsLoacation")]
        public async Task<IEnumerable<SensorLocationMinDto>> GetAllSensors() //просто получаю данные датчиков и маплю в дто
        {
            var sensors = await _sensorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SensorLocationMinDto>>(sensors);
        }
        
    }
}
