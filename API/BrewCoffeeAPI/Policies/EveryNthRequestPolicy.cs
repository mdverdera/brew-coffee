using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;

namespace BrewCoffeeAPI.Policies
{
    public class EveryNthRequestPolicy : IAvailabilityPolicy
    {
        private readonly ICounterService _counterService;
        private readonly int _n;
        public EveryNthRequestPolicy(ICounterService counterService, int n = 5)
        {
            _counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            _n = n;
        }
        public BrewCoffeeResultModel? Evaluate(double lat, double lon, DateTime requestDate)
        {
            var count = _counterService.Increment();
            if (count % _n == 0)
            {
                return new BrewCoffeeResultModel { StatusCode = 503 };
            }

            return null;
        }
    }
}
