using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrewCoffeeAPI.Mappers
{
    public class BrewCoffeeResultMapper : IBrewCoffeeResultMapper
    {
        public IActionResult Map(BrewCoffeeResultModel result)
        {
            if (result is null)
                return new StatusCodeResult(500);

            return result.StatusCode switch
            {
                200 => new OkObjectResult(new { message = result.Message, prepared = result.Prepared }),
                503 => new StatusCodeResult(503),
                418 => new StatusCodeResult(418),
                _ => new StatusCodeResult(500)
            };
        }
    }
}
