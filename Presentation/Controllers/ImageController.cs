using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IDogRepository _dogRepository;
        private readonly IOpenAiService _openAiService;
        public ImageController(IImageService imageService, IDogRepository dogRepository, IOpenAiService openAiService)
        {
            _imageService = imageService;
            _dogRepository = dogRepository;
            _openAiService = openAiService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(IFormFile file, [FromQuery] int entityId, int? entityType)
        {
            if (entityType == 1 && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.UploadImage(file, entityId, entityType);

                if (ParkResult == null)
                {
                    return BadRequest("park bad");
                }
                return Ok(ParkResult);
            }
            else if (entityType == 1 || entityType == null)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0 || entityType != 0)
            {
                return Unauthorized();
            }

            string? dogResult = await _imageService.UploadImage(file, entityId, entityType);
            Console.WriteLine(entityId + " " + entityType.ToString());
            if (dogResult == null)
            {
                return BadRequest("dog result bad");
            }

            if (await _imageService.IsFirstImage(entityId))
            {
                DogBreedSize AiResult = await _openAiService.GetDogBreedFromImageUrlAsync(dogResult);
                if (AiResult.Size != -1 && AiResult.Breed != -1)
                {
                    Dog? dogToUpdate = await _dogRepository.GetDogByIdDb(dogId);
                    dogToUpdate.Type = (DogType)AiResult.Breed;
                    dogToUpdate.Size = (DogSize)AiResult.Size;
                    await _dogRepository.UpdateDogDb(dogId, dogToUpdate);
                }
            }
            return Ok(dogResult);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromQuery] int imageId1, [FromQuery] int imageId2, [FromQuery] int? entityType, [FromQuery] int entityId)
        {
            if (entityType == 1 && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.ReOrderImages(imageId1, imageId2);

                if (ParkResult == null)
                {
                    return BadRequest();
                }
                return Ok(ParkResult);
            }
            else if (entityType == 1 || entityType == null)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0 || entityType != 0)
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int imageId, [FromQuery] int entityId, [FromQuery] int? entityType)
        {
            if (entityType == 1 && HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value == "True")
            {
                string? ParkResult = await _imageService.RemoveImage(imageId, entityId, entityType);

                if (ParkResult == null)
                {
                    return BadRequest();
                }
                return Ok(ParkResult);
            }
            else if (entityType == 1 || entityType == null)
            {
                return Unauthorized();
            }

            int dogId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == entityId.ToString())?.Value, out dogId);

            if (dogId == 0 || entityType != 0)
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
