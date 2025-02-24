using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

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
        // להוסיף לוג.אין מודל
        // יהיה בנוי מאימייל + סיסמא או לחלופין אימייל + מגוגל איי.די
        // להוסיף קבלת סיסמא בהתחברות
        public async Task<IActionResult> Login(string email)
        {
            string? result = await _userService.Login(email);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpGet]
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
        public async Task<IActionResult> GetUserById(string id)
        {
            User? result = await _userService.GetOne(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User? result = await _userService.Update(id, user);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            User? result = await _userService.Delete(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
