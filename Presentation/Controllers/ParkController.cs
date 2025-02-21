using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

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
            public async Task<IActionResult> Post([FromBody] Park park)
            {
                Park? result = await _parkService.CreateParkDb(park);
                //if (!ModelState.IsValid)
                if (park == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }

          

            [HttpGet]
            public async Task<IActionResult> GetAllParks()
            {
                List<Park> result = await _parkService.GetAllParksDb();
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetParkById(string id)
            {
                Park? result = await _parkService.GetParkByIdDb(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Put(string id, [FromBody] Park park)
            {
                Park? result = await _parkService.UpdateParkDb(id, park);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                Park? result = await _parkService.DeleteParkDb(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }

             [HttpPost("{parkId}")]
         public async Task<IActionResult> Post(string parkId, [FromBody] string dogId)
         {
            Park? result = await _parkService.LikeUnlikeParkDb(parkId, dogId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
         }
    }
    }

