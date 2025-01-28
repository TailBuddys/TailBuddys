using Microsoft.AspNetCore.Mvc;

namespace TailBuddys.Presentation.Controllers
{
    public class ParkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
