using Microsoft.AspNetCore.Mvc;

namespace Fix_All.Controllers
{
    public class LaberPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
