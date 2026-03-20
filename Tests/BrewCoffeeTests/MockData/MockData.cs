using BrewCoffeeAPI.Models;

namespace BrewCoffeeTests.MockData
{
    public static class MockData
    {
        public static WeatherResponseModel CreateTempMock(double temp)
        {
            return new WeatherResponseModel
            {
                main = new MainModel
                {
                    temp = temp
                }
            };
        }
    }
}
