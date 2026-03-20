namespace BrewCoffeeAPI.Models
{
    public class WeatherResponseModel
    {
        public MainModel main { get; set; }
    }
    public class MainModel
    {
        public double temp { get; set; }
    }
}
