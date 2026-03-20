using BrewCoffeeAPI.Brew;
using BrewCoffeeAPI.Interfaces;
using Moq;
using System.Reflection.Metadata;

namespace BrewCoffeeTests
{
    public class BrewCoffeeHandlerTests
    {
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Mock<IWeatherService> _weatherService;
        private readonly GetBrewCoffeeHandler _handler;

        public BrewCoffeeHandlerTests()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _handler = new GetBrewCoffeeHandler(_dateTimeProvider.Object, _weatherService.Object);
        }

        [Fact]
        public async Task BrewCoffee_ExecuteGet_ReturnStatus200()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 19, 10, 56, 24));

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

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

            for (int i = 0; i < 4; i++)
            {
                await _handler.Handle(new BrewCoffeeQuery(), default);
            }

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

            Assert.Equal(503, result.StatusCode);
            Assert.Null(result.Message);
            Assert.Null(result.Prepared);
        }

        [Fact]
        public async Task BrewCoffee_April1st_Return418()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 4, 1));

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

            Assert.Equal(418, result.StatusCode);
            Assert.Null(result.Message);
            Assert.Null(result.Prepared);
        }

        [Fact]
        public async Task BrewCoffee_April1stAllCall_Return418()
        {
            _dateTimeProvider.Setup(x => x.Now)
                .Returns(new DateTime(2026, 4, 1));

            for (int i = 0; i < 5; i++)
            {
                var result = await _handler.Handle(new BrewCoffeeQuery(), default);
                Assert.Equal(418, result.StatusCode);
            }
        }

        [Fact]
        public async Task BrewCoffee_TemperatureAbove30_ReturnIcedCoffee()
        {
            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync())
                .ReturnsAsync(31);

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Your refreshing iced coffee is ready", result.Message);
        }

        [Fact]
        public async Task BrewCoffee_TemperatureBelow30_ReturnHotCoffee()
        {
            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync())
                .ReturnsAsync(25);

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

            Assert.Equal("Your piping hot coffee is ready", result.Message);
        }

        [Fact]
        public async Task BrewCoffee_TemperatureEqual30_ReturnHotCoffee()
        {
            var dateMock = new Mock<IDateTimeProvider>();
            dateMock.Setup(x => x.Now)
                .Returns(new DateTime(2026, 3, 20));

            _weatherService.Setup(x => x.GetTemperatureAsync())
                .ReturnsAsync(30);

            var result = await _handler.Handle(new BrewCoffeeQuery(), default);

            Assert.Equal("Your piping hot coffee is ready", result.Message);
        }
    }
}