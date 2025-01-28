using Microsoft.AspNetCore.Mvc;

namespace TailBuddys.Presentation.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
