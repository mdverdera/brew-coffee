using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using MediatR;

namespace BrewCoffeeAPI.Brew
{
    public record BrewCoffeeQuery() : IRequest<BrewCoffeeResultModel>;
    public class GetBrewCoffeeHandler : IRequestHandler<BrewCoffeeQuery, BrewCoffeeResultModel>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWeatherService _weatherService;
        private static int _counter = 0;

        public GetBrewCoffeeHandler(IDateTimeProvider dateTimeProvider, IWeatherService weatherService)
        {
            _dateTimeProvider = dateTimeProvider;
            _weatherService = weatherService;
        }

        public async Task<BrewCoffeeResultModel> Handle(BrewCoffeeQuery request, CancellationToken cancellationToken)
        {
            var april1stConfig = false; // Set to true to simulate April 1st behavior for testing purposes
            var now = april1stConfig ? new DateTime(2026, 4, 1) : _dateTimeProvider.Now; 

            if (now.Month == 4 && now.Day == 1)
            {
                return new BrewCoffeeResultModel
                {
                    StatusCode = 418
                };
            }

            var count = Interlocked.Increment(ref _counter);

            if (count % 5 == 0)
            {
                return new BrewCoffeeResultModel
                {
                    StatusCode = 503
                };
            }

            var temperature = await _weatherService.GetTemperatureAsync();

            var message = temperature > 30
                ? "Your refreshing iced coffee is ready"
                : "Your piping hot coffee is ready";

            return new BrewCoffeeResultModel
            {
                StatusCode = 200,
                Message = message,
                Prepared = now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };
        }
    }
}

