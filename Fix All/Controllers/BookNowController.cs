using Microsoft.AspNetCore.Mvc;

namespace Fix_All.Controllers
{
    public class BookNowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
