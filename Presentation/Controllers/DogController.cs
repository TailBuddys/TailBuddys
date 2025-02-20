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
        public async Task<IActionResult> Post([FromBody] Dog dog, string userId)
        {
            Dog? result = await _dogService.Create(dog, userId);
            //if (!ModelState.IsValid)
            if (dog == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
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
        public async Task<IActionResult> GetAllUserDogs(string userId)
        {
            List<Dog> result = await _dogService.GetAll(userId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDogById(string id)
        {
            Dog? result = await _dogService.GetOne(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Dog dog)
        {
            Dog? result = await _dogService.Update(id, dog);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Dog? result = await _dogService.Delete(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
