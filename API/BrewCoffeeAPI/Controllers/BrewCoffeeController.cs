using BrewCoffeeAPI.Brew;
using BrewCoffeeAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrewCoffeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBrewCoffeeResultMapper _resultMapper;
        public BrewCoffeeController(IMediator mediator, IBrewCoffeeResultMapper resultMapper)
        {
            _mediator = mediator;
            _resultMapper = resultMapper;
        }

        [HttpGet("/brew-coffee")]
        public async Task<IActionResult> BrewCoffee(
            [FromQuery] double? lat = null,
            [FromQuery] double? lon = null,
            [FromQuery] DateTime? date = null
            )
        {
            var result = await _mediator.Send(new BrewCoffeeQuery(lat, lon, date));
            return _resultMapper.Map(result);
        }
    }
}
