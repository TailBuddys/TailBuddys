using Microsoft.AspNetCore.Mvc;

namespace TailBuddys.Presentation.Controllers
{
    public class DogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
