using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Dog dog, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? ClientId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (ClientId != null && ClientId == userId)
            {
                Dog? result = await _dogService.Create(dog, userId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();
        }

        [HttpGet]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> GetAllDogs()
        {
            List<Dog> result = await _dogService.GetAll();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllUserDogs(string userId)
        {
            string? clientId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || clientId == userId)
            {
                List<Dog> result = await _dogService.GetAll(userId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDogById(string id)
        {
            Dog? result = await _dogService.GetOne(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("match/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUnmatchedDogs(string id)
        {
            string? dogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id)?.Value;

            if (dogId == id)
            {
                List<Dog> result = await _dogService.GetUnmatchedDogs(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(string id, [FromBody] Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? dogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id)?.Value;
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || dogId == id)
            {
                Dog? result = await _dogService.Update(id, dog);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            string? dogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id)?.Value;
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || dogId == id)
            {
                Dog? result = await _dogService.Delete(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();
        }
    }
}
