

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestTaskApi.Controllers;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Models.Transfer;
using TestTaskApi.Parser.Service;

namespace UnitTests
{
    [TestClass]
    public class SensorControllerTest
    {
        private SensorController _controller;
        private Mock<ISensorRepository> _mockSensorRepository;
        private Mock<IParserService> _mockParserService;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void Setup()
        {
            _mockSensorRepository = new Mock<ISensorRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockParserService = new Mock<IParserService>();
            _controller = new SensorController(
                _mockSensorRepository.Object,
                _mockParserService.Object,
                _mockMapper.Object
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
        
            var fileMock = new Mock<IFormFile>();
            var content = "DateTime;SensorName;CustomerName;Flags;SensorType;Unit;East;North;Height;km;VALUE1;VALUE2\n28.06.2022 15:30:31;TurbineMonitoring:DH_HS5_HS2_E2_comp;;;HydrostaticLevel;Hectopascal;0.0000;0.0000;0.0000;0.0000;0.0;15.0";

            var sensor = new Sensor
            {
                DateTime = new string[] { "28.06.2022 15:30:31" },
                SensorName = "TurbineMonitoring:DH_HS5_HS2_E2_comp",
                CustomerName = "",
                Flags = "",
                SensorType = "HydrostaticLevel",
                Unit = "Hectopascal",
                East = 0,
                North = 0,
                Height = 0,
                Km = 0,
                Values1 = new double[] { 0 },
                Values2 = new double[] { 15 },
            };

            var byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(byteArray);

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.ContentType).Returns("test/csv");
            fileMock.Setup(f => f.FileName).Returns("test.csv");

            _mockParserService.Setup(p => p.ParseAsync<Sensor>(content)).ReturnsAsync(sensor);
            _mockSensorRepository.Setup(repo => repo.AddAsync(It.IsAny<Sensor>())).Returns(Task.CompletedTask);

            var result = await _controller.LoadSensorData(fileMock.Object);

            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }
        [TestMethod]
        public async Task GetAllSensors_200_ReturnDto()
        {
            
            var sensors = new List<Sensor>
            {
                new Sensor
                {
                DateTime = new string[] { "28.06.2022 15:30:31" },
                SensorName = "TurbineMonitoring:DH_HS5_HS2_E2_comp",
                CustomerName = "",
                Flags = "",
                SensorType = "HydrostaticLevel",
                Unit = "Hectopascal",
                East = 0,
                North = 0,
                Height = 0,
                Km = 0,
                Values1 = new double[] { 0 },
                Values2 = new double[] { 15 },
            }
            };
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
            _mockSensorRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(sensors);
            _mockMapper.Setup(m => m.Map<IEnumerable<SensorLocationMinDto>>(sensors)).Returns(sensorDtos);


            var result = await _controller.GetAllSensors() as IEnumerable<SensorLocationMinDto>;
           
            Assert.IsNotNull(result);
            Assert.AreEqual(sensorDtos.Count, result.Count());
        }
    }
}
