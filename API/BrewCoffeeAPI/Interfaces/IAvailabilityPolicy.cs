using BrewCoffeeAPI.Models;

namespace BrewCoffeeAPI.Interfaces
{
    public interface IAvailabilityPolicy
    {
        BrewCoffeeResultModel? Evaluate(double lat, double lon, DateTime requestDate);
    }
}
