using Microsoft.AspNetCore.Mvc;

namespace TailBuddys.Presentation.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
