using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParkController : Controller
    {
        private readonly IParkService _parkService;
        public ParkController(IParkService parkService)
        {
            _parkService = parkService;
        }

        [HttpPost]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Post([FromBody] Park park)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Park? result = await _parkService.CreatePark(park);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllParks([FromQuery] ParksFilterDTO filters)
        {
            List<ParkDTO> result = await _parkService.GetAllParks(null, filters);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("dog/{dogId}")]
        [Authorize]
        public async Task<IActionResult> GetAllParks(int dogId, [FromQuery] ParksFilterDTO filters)
        {
            int clientDogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId.ToString())?.Value, out clientDogId);

            if (dogId != 0)
            {
                List<ParkDTO> result = await _parkService.GetAllParks(dogId, filters);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParkById(int id)
        {
            Park? result = await _parkService.GetParkById(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Put(int id, [FromBody] Park park)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Park? result = await _parkService.UpdatePark(id, park);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            Park? result = await _parkService.DeletePark(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("{parkId}")]
        [Authorize]
        public async Task<IActionResult> Post(int parkId, [FromBody] int dogId)
        {
            int clientDogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId.ToString())?.Value, out clientDogId);

            if (clientDogId == dogId && clientDogId != 0)
            {
                Park? result = await _parkService.LikeUnlikePark(parkId, dogId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();

        }
    }
}

