using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User? result = await _userService.Register(user);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? result = await _userService.Login(loginModel);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> result = await _userService.GetAll();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            int userId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out userId);
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || (userId == id && userId != 0))
            {
                User? result = await _userService.GetOne(id);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int userId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out userId);
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || (userId == id && userId != 0))
            {
                User? result = await _userService.Update(id, user);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            int userId;
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out userId);
            string? isUserAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;

            if (isUserAdmin == "True" || (userId == id && userId != 0))
            {
                User? result = await _userService.Delete(id);
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
