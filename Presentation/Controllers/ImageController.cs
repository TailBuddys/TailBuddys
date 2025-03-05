using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] IFormFile file, int entityId, EntityType entityType)
        {
            if (entityType == EntityType.Park && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.UploadImage(file, entityId, entityType);

                if (ParkResult == null)
                {
                    return BadRequest();
                }
                return Ok(ParkResult);
            }
            else if (entityType == EntityType.Park)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0)
            {
                return Unauthorized();
            }

            string? dogResult = await _imageService.UploadImage(file, entityId, entityType);

            if (dogResult == null)
            {
                return BadRequest();
            }
            return Ok(dogResult);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] int imageId1, int imageId2, EntityType entityType, int entityId)
        {
            if (entityType == EntityType.Park && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.ReOrderImages(imageId1, imageId2);

                if (ParkResult == null)
                {
                    return BadRequest();
                }
                return Ok(ParkResult);
            }
            else if (entityType == EntityType.Park)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0)
            {
                return Unauthorized();
            }

            string? dogResult = await _imageService.ReOrderImages(imageId1, imageId2);

            if (dogResult == null)
            {
                return BadRequest();
            }
            return Ok(dogResult);
        }

        [HttpDelete("{chatId}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] int imageId, int entityId, EntityType entityType)
        {
            if (entityType == EntityType.Park && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.RemoveImage(imageId, entityId, entityType);

                if (ParkResult == null)
                {
                    return BadRequest();
                }
                return Ok(ParkResult);
            }
            else if (entityType == EntityType.Park)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0)
            {
                return Unauthorized();
            }

            string? dogResult = await _imageService.RemoveImage(imageId, entityId, entityType);

            if (dogResult == null)
            {
                return BadRequest();
            }
            return Ok(dogResult);
        }
    }
}
