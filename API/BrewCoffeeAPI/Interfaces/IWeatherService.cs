namespace BrewCoffeeAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<double> GetTemperatureAsync();
    }
}
