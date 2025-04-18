using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int dogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == match.SenderDogId.ToString())?.Value, out dogId);

            if (dogId != 0)
            {
                Match? result = await _matchService.CreateMatch(match);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllMutualMatches(int dogId)
        {
            int clientDogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId.ToString())?.Value, out clientDogId);

            if (clientDogId != 0)
            {
                List<MatchDTO> result = await _matchService.GetAllMutualMatches(dogId);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        // לא בטוח שנצטרך בכלל
        //[HttpGet("{matchId}")]
        //[Authorize(Policy = "MustHaveDog")]
        //public async Task<IActionResult> GetMatchById(int matchId)
        //{
        //    string? clientDogId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "DogId" && c.Value == dogId)?.Value;

        //    if (clientDogId == dogId)
        //    {
        //        Match? result = await _matchService.GetMatchById(matchId);
        //        if (result == null)
        //        {
        //            return BadRequest();
        //        }
        //        return Ok(result);
        //    }
        //    return Unauthorized();
        //}

        [HttpPut("{matchId}")]
        [Authorize]
        public async Task<IActionResult> Put(int matchId, [FromBody] Match newMatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int SenderDogId;
            int.TryParse(HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == "DogId" && c.Value == newMatch.SenderDogId.ToString())?.Value, out SenderDogId);

            if (SenderDogId != 0)
            {
                Match? matchToUpdate = await _matchService.GetMatchById(matchId);
                if (matchToUpdate == null || matchToUpdate.SenderDogId != SenderDogId)
                {
                    return Unauthorized();
                }

                Match? result = await _matchService.UpdateMatch(matchId, newMatch);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            else return Unauthorized();

        }

        // ככל הנראה נרצה להפוך מאץ' ללא פעיל

        [HttpDelete("{matchId}")]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Delete(int matchId)
        {
            Match? result = await _matchService.DeleteMatch(matchId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}

