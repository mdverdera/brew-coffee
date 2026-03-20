using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using System.Text.Json;
using static BrewCoffeeAPI.Commons.Constants;

namespace BrewCoffeeAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherResponseModel> GetTemperatureAsync(double lat, double lon)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
            var client = _httpClientFactory.CreateClient("OpenWeather");

            var response = await client.GetAsync(
                $"{OpenWeatherEndpoints.CurrentWeather}?lat={lat}&lon={lon}&units=metric&APPID={apiKey}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Received weather data: {json}");

            var data = JsonSerializer.Deserialize<WeatherResponseModel>(json);

            return data;
        }
    }
}
