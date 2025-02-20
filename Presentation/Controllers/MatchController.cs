using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
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
        public async Task<IActionResult> Post([FromBody] Match match)
        {
            Match? result = await _matchService.CreateMatch(match);
            //if (!ModelState.IsValid)
            if (match == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMutualMatches(string dogId)
        {
            List<Match> result = await _matchService.GetAllMutualMatches(dogId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{matchId}")]
        public async Task<IActionResult> GetMatchById(int matchId)
        {
            Match? result = await _matchService.GetMatchById(matchId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut("{matchId}")]
        public async Task<IActionResult> Put(int matchId, [FromBody] Match newMatch)
        {
            Match? result = await _matchService.UpdateMatch(matchId, newMatch);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{matchId}")]
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

