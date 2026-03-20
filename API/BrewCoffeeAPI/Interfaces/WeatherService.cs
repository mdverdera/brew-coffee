using BrewCoffeeAPI.Models;
using System.Diagnostics;
using System.Text.Json;

namespace BrewCoffeeAPI.Interfaces
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetTemperatureAsync()
        {
            var response = await _httpClient.GetAsync("https://api.openweathermap.org/data/3.0/onecall?lat=33.44&lon=-94.04&appid=7d09ac2bc8c2adde406121c57198eca7");

            response.EnsureSuccessStatusCode();

            var resultStream = await response.Content.ReadAsStreamAsync();
            Debug.WriteLine($"Received weather data: {new StreamReader(resultStream).ReadToEnd()}");
            var data = JsonSerializer.DeserializeAsync<WeatherResponseModel>(resultStream);

            return ??; //TODO: need to subscribe to OpenWeather for free API calls
        }        
    }
}
