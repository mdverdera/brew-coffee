using BrewCoffeeAPI.Interfaces;

namespace BrewCoffeeAPI.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
