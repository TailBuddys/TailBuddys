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
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        public DogController(IDogService dogService)
        {
            _dogService = dogService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");

            if (userId == 0)
            {
                return Unauthorized();
            }

            DogDTO? result = await _dogService.Create(dog, userId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
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

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetAllUserDogs()
        {
            int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");

            if (userId == 0)
            {
                return Unauthorized();
            }

            List<UserDogDTO> result = await _dogService.GetAll(userId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDogById(int id)
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
        public async Task<IActionResult> GetUnmatchedDogs(int id, [FromQuery] DogsFilterDTO filters)
        {
            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id.ToString())?.Value, out dogId);

            if (dogId == id)
            {
                List<DogDTO> result = await _dogService.GetUnmatchedDogs(id, filters);
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
        public async Task<IActionResult> Put(int id, [FromBody] Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id.ToString())?.Value, out dogId);

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
        public async Task<IActionResult> Delete(int id)
        {
            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == id.ToString())?.Value, out dogId);

            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || dogId == id)
            {
                DogDTO? result = await _dogService.Delete(id);
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
