using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Fix_All.Models;

namespace Fix_All.Controllers
{
    public class LaberPanelController : Controller
    {
        private readonly mydbcontext _context;

        public LaberPanelController(mydbcontext context)
        {
            _context = context;
        }
        public IActionResult laber_panel_Login()
        {
            return View();
        }
        // POST Login
        [HttpPost]
        public IActionResult laber_panel_Login(string Email, string PasswordHash)
        {
            var user = _context.approve_labers
                               .FirstOrDefault(u => u.Email == Email && u.PasswordHash == PasswordHash && u.Status == "Approved");

            if (user != null)
            {
                // ✅ Set session
                HttpContext.Session.SetInt32("LaberId", user.ApproveLarberId);
                HttpContext.Session.SetString("LaberName", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("LaberEmail", user.Email);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Invalid Email or Password, or your account is not approved.";
                return View();
            }
        }
        public IActionResult Index()
        {
            int? laberId = HttpContext.Session.GetInt32("LaberId");

            if (laberId == null)
            {
                return RedirectToAction("laber_panel_Login");
            }

            var laber = _context.approve_labers.Find(laberId);
            return View(laber); // send laber data to dashboard
            
        }
        public IActionResult viewprofile()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("laber_panel_Login");
        }
    }
}
