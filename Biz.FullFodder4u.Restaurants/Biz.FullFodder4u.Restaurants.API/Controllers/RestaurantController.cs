using Biz.FullFodder4u.Restaurants.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biz.FullFodder4u.Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private static readonly IList<RestaurantDto> restaurants = new List<RestaurantDto> 
        { 
            new RestaurantDto { Id = Guid.NewGuid(), Name = "PizzaHut" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Halo Pizza" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Polne smaki" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Chinol z OBI" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Plackowa izba" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "KFC" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Kilo mąki" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Pierogi polskie" },
            new RestaurantDto { Id = Guid.NewGuid(), Name = "Pierogowa izba" }
        };

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Ok(restaurants);
            }    

            var result = restaurants.Where(r => r.Name.ToLower().StartsWith(name.ToLower()));

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(RestaurantDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> CreateAsync(string name)
        {
            if (!restaurants.Any(r => r.Name.ToLower() == name.ToLower()))
            {
                var restaurant = new RestaurantDto { Id = Guid.NewGuid(), Name = name };
                restaurants.Add(restaurant);

                return CreatedAtAction(nameof(GetAsync), new { id = restaurant.Id }, restaurant);
            }

            return NoContent();
        }
    }
}
