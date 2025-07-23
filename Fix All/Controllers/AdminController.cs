using Microsoft.AspNetCore.Mvc;

namespace Fix_All.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Table()
        {
            return View();
        }
    }
}
