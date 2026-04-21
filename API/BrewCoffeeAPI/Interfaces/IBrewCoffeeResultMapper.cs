using BrewCoffeeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrewCoffeeAPI.Interfaces
{
    public interface IBrewCoffeeResultMapper
    {
        IActionResult Map(BrewCoffeeResultModel result);
    }
}
