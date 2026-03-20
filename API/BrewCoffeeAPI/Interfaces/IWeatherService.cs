using BrewCoffeeAPI.Models;

namespace BrewCoffeeAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseModel> GetTemperatureAsync();
    }
}
