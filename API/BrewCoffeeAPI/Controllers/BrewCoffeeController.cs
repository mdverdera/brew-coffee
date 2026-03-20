using BrewCoffeeAPI.Brew;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrewCoffeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrewCoffeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/brew-coffee")]
        public async Task<IActionResult> BrewCoffee(
            [FromQuery] double? lat = null,
            [FromQuery] double? lon = null,
            [FromQuery] DateTime? date = null
            )
        {
            var result = await _mediator.Send(new BrewCoffeeQuery
            (
                lat,
                lon,
                date
            ));

            return result.StatusCode switch
            {
                200 => Ok(new
                {
                    message = result.Message,
                    prepared = result.Prepared
                }),
                503 => StatusCode(503),
                418 => StatusCode(418),
                _ => StatusCode(500)
            };
        }
    }
}
