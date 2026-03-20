using BrewCoffeeAPI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace BrewCoffeeAPI.Interfaces
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherResponseModel> GetTemperatureAsync()
        {
            var client = _httpClientFactory.CreateClient("OpenWeather");

            var response = await client.GetAsync(
                "data/2.5/weather?lat=14.6581&lon=121.0546&units=metric&APPID=7d09ac2bc8c2adde406121c57198eca7");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Received weather data: {json}");

            var data = JsonSerializer.Deserialize<WeatherResponseModel>(json);

            return data;
        }
    }
}
