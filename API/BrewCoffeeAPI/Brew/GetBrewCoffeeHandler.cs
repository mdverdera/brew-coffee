using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using MediatR;
using static BrewCoffeeAPI.Commons.Constants;

namespace BrewCoffeeAPI.Brew
{
    public record BrewCoffeeQuery(double? lat, double? lon, DateTime? date) : IRequest<BrewCoffeeResultModel>;
    public class GetBrewCoffeeHandler : IRequestHandler<BrewCoffeeQuery, BrewCoffeeResultModel>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWeatherService _weatherService;
        private readonly IAvailabilityPolicy _availabilityPolicy;

        public GetBrewCoffeeHandler(IDateTimeProvider dateTimeProvider, IWeatherService weatherService, IAvailabilityPolicy availabilityPolicy)
        {
            _dateTimeProvider = dateTimeProvider;
            _weatherService = weatherService;
            _availabilityPolicy = availabilityPolicy;
        }

        public async Task<BrewCoffeeResultModel> Handle(BrewCoffeeQuery request, CancellationToken cancellationToken)
        {
            var lat = request.lat ?? 14.6583; //I just input my location as default
            var lon = request.lon ?? 121.0547; //I just input my location as default
            var dateRequest = request.date ?? _dateTimeProvider.Now; //to override current date for testing purposes 

            if (dateRequest.Month == 4 && dateRequest.Day == 1)
            {
                return new BrewCoffeeResultModel
                {
                    StatusCode = 418
                };
            }

            var policyResult = _availabilityPolicy.Evaluate(lat, lon, dateRequest);
            if (policyResult != null)
            {
                return policyResult;
            }

            var temperature = await _weatherService.GetTemperatureAsync(lat, lon);

            var message = temperature.main.temp > 30
                ? Messages.IcedCoffee
                : Messages.HotCoffee;

            return new BrewCoffeeResultModel
            {
                StatusCode = 200,
                Message = message,
                Prepared = dateRequest.ToString("yyyy-MM-ddTHH:mm:sszzz")
            };
        }
    }
}

