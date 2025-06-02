

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TestTaskApi.Controllers;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Mapping;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Models.Transfer;
using TestTaskApi.Parser.Service;
using TestTaskApi.Parser.Variation;

namespace UnitTests
{
    [TestClass]
    public class SensorControllerTest
    {
        private SensorController _controller;
        private  ISensorRepository _sensorRepository; 
        private IParserService _parser; 
        private IMapper _mapper;
        private IMongoDb _mongoDb;
        private IFormFile MockFile
        {
            get
            {
                var content = "DateTime;SensorName;CustomerName;Flags;SensorType;Unit;East;North;Height;km;VALUE1;VALUE2\n28.06.2022 15:30:31;TurbineMonitoring:DH_HS5_HS2_E2_comp;;;HydrostaticLevel;Hectopascal;0.0000;0.0000;0.0000;0.0000;0.0;15.0";
                var fileMock = new Mock<IFormFile>();

                var byteArray = System.Text.Encoding.UTF8.GetBytes(content);
                var stream = new MemoryStream(byteArray);

                fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
                fileMock.Setup(f => f.Length).Returns(stream.Length);
                fileMock.Setup(f => f.ContentType).Returns("test/csv");
                fileMock.Setup(f => f.FileName).Returns("test.csv");
                return fileMock.Object;
            }
        }
        
        [TestInitialize]
        public void Setup()
        {
            _mongoDb = new MongoDbService("mongodb://localhost/UnitTest");
            _sensorRepository = new SensorRepository(_mongoDb);

            _sensorRepository.ClearAsync().GetAwaiter().GetResult(); 

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            
            _parser = new OneEntityParserService();
            _controller = new SensorController(
                _sensorRepository,
                _parser,
               _mapper
                );
        }
        
       
        [TestMethod]
        public async Task LoadData_500_WhenNull()
        {
            IFormFile file = null;
            var result = await _controller.LoadSensorData(file);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public async Task LoadData_500_WhenFileEmpty()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            var result = await _controller.LoadSensorData(mockFile.Object);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public async Task LoadData_204_WhenFileIsValid()
        {                             
            var result = await _controller.LoadSensorData(MockFile);

            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }
        [TestMethod]
        public async Task GetAllSensors_200_ReturnDto()
        {
                        
            var sensorDtos = new List<SensorLocationMinDto>
            {
                new SensorLocationMinDto
                {
                    Sensor_name = "TurbineMonitoring:DH_HS5_HS2_E2_comp",
                    North = 0,
                    East = 0,
                    Values = new double[] { 0 },
                }
            };

            await _controller.LoadSensorData(MockFile);

            await Task.Delay(1000);

            var result = await _controller.GetAllSensors() as IEnumerable<SensorLocationMinDto>;            
            
            Assert.IsNotNull(result);
            var resultList = result.ToList();

            Assert.AreEqual(sensorDtos.Count, resultList.Count, "Dto count not equal");

            for (int i = 0; i < sensorDtos.Count; i++)
            {
                Assert.AreEqual(sensorDtos[i].Sensor_name, resultList[i].Sensor_name, $"Sensor_name not equal on position {i}.");
                Assert.AreEqual(sensorDtos[i].North, resultList[i].North, $"North not equal on position {i}.");
                Assert.AreEqual(sensorDtos[i].East, resultList[i].East, $"East not equal on position {i}.");              
                CollectionAssert.AreEqual(sensorDtos[i].Values, resultList[i].Values, $"Values not equal on position {i}.");
            }
        }       
    }
}
