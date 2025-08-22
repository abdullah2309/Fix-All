using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Fix_All.Models;
using System.Text;
using System.Security.Cryptography;
using System.Text;

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
        public static class PasswordHelper
        {
            public static string HashPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sb = new StringBuilder();
                    foreach (var b in bytes)
                        sb.Append(b.ToString("x2")); // convert to hex
                    return sb.ToString();
                }
            }
        }
        [HttpPost]
        public IActionResult laber_panel_Login(string Email, string PasswordHash)
        {
            // Hash user input
            string hashedInput = PasswordHelper.HashPassword(PasswordHash);

            var user = _context.approve_labers
                               .FirstOrDefault(u => u.Email == Email &&
                                                    u.PasswordHash == hashedInput &&
                                                    u.Status == "Approved");

            if (user != null)
            {
                // ✅ Set session
                HttpContext.Session.SetInt32("LaberId", user.ApproveLarberId);
                HttpContext.Session.SetString("LaberName", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("LaberEmail", user.Email);

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Error = "Invalid Email/Password or not approved yet.";
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
