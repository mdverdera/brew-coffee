using BrewCoffeeAPI.Brew;
using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using Moq;

namespace BrewCoffeeTests
{
    public class BrewCoffeeHandlerTests
    {
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Mock<IWeatherService> _weatherService;
        private readonly Mock<IAvailabilityPolicy> _availabilityPolicy;
        private readonly GetBrewCoffeeHandler _handler;

        public BrewCoffeeHandlerTests()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _weatherService = new Mock<IWeatherService>();
            _availabilityPolicy = new Mock<IAvailabilityPolicy>();
            _handler = new GetBrewCoffeeHandler(_dateTimeProvider.Object, _weatherService.Object, _availabilityPolicy.Object);
        }

        [Fact]
        public async Task BrewCoffee_ExecuteGet_ReturnStatus200()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 19, 10, 56, 24));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(24));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns((BrewCoffeeResultModel?)null);

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Prepared);
            Assert.Equal("Your piping hot coffee is ready", result.Message);
            Assert.StartsWith("2026-03-19T10:56:24", result.Prepared);
        }

        [Fact]
        public async Task BrewCoffee_5thCall_Return503()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 19));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(24));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns(new BrewCoffeeResultModel { StatusCode = 503 });

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal(503, result.StatusCode);
            Assert.Null(result.Message);
            Assert.Null(result.Prepared);
        }

        [Fact]
        public async Task BrewCoffee_April1st_Return418()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 4, 1));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(24));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns((BrewCoffeeResultModel?)null);

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal(418, result.StatusCode);
            Assert.Null(result.Message);
            Assert.Null(result.Prepared);
        }

        [Fact]
        public async Task BrewCoffee_April1stAllCall_Return418()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 4, 1));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(24));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns(new BrewCoffeeResultModel { StatusCode = 503 });

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);
            Assert.Equal(418, result.StatusCode);

        }

        [Fact]
        public async Task BrewCoffee_TemperatureAbove30_ReturnIcedCoffee()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(32));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns((BrewCoffeeResultModel?)null);

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Your refreshing iced coffee is ready", result.Message);
        }

        [Fact]
        public async Task BrewCoffee_TemperatureBelow30_ReturnHotCoffee()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(24));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns((BrewCoffeeResultModel?)null);

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal("Your piping hot coffee is ready", result.Message);
        }

        [Fact]
        public async Task BrewCoffee_TemperatureEqual30_ReturnHotCoffee()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(MockData.MockData.CreateTempMock(30));

            _availabilityPolicy
                .Setup(x => x.Evaluate(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
                .Returns((BrewCoffeeResultModel?)null);

            var result = await _handler.Handle(new BrewCoffeeQuery(It.IsAny<double>(), It.IsAny<double>(), null), default);

            Assert.Equal("Your piping hot coffee is ready", result.Message);
        }
    }
}