using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using MediatR;

namespace BrewCoffeeAPI.Brew
{
    public record BrewCoffeeQuery() : IRequest<BrewCoffeeResultModel>;
    public class GetBrewCoffeeHandler : IRequestHandler<BrewCoffeeQuery, BrewCoffeeResultModel>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private static int _counter = 0;

        public GetBrewCoffeeHandler(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<BrewCoffeeResultModel> Handle(BrewCoffeeQuery request, CancellationToken cancellationToken)
        {
            var april1stConfig = false; // Set to true to simulate April 1st behavior for testing purposes
            var now = april1stConfig ? new DateTime(2026, 4, 1) : _dateTimeProvider.Now; 

            if (now.Month == 4 && now.Day == 1)
            {
                return Task.FromResult(new BrewCoffeeResultModel
                {
                    StatusCode = 418
                });
            }

            var count = Interlocked.Increment(ref _counter);

            if (count % 5 == 0)
            {
                return Task.FromResult(new BrewCoffeeResultModel
                {
                    StatusCode = 503
                });
            }

            return Task.FromResult(new BrewCoffeeResultModel
            {
                StatusCode = 200,
                Message = "Your piping hot coffee is ready",
                Prepared = now.ToString("yyyy-MM-ddTHH:mm:sszzz")
            });
        }
    }
}

