using Microsoft.AspNetCore.Mvc;

namespace Fix_All.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
