using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fix_All.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly mydbcontext _context;

        public HomeController(ILogger<HomeController> logger, mydbcontext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult LaborSignin()
        {
            ViewBag.Fields = new SelectList(_context.LaborFields, "FieldId", "Name");
            return View();
        }

        //public IActionResult Register()
        //{
        //    ViewBag.Fields = new SelectList(_context.LaborFields, "FieldId", "Name");
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Register(ServiceProvider model, IFormFile CVFile, IFormFile ProfileImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Fields = new SelectList(_context.LaborFields, "FieldId", "Name");
                return View(model);
            }

            // Save files
            if (CVFile != null && CVFile.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/uploads/cv", CVFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await CVFile.CopyToAsync(stream);
                model.CVFilePath = "/uploads/cv/" + CVFile.FileName;
            }

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var imgPath = Path.Combine("wwwroot/uploads/images", ProfileImage.FileName);
                using var stream = new FileStream(imgPath, FileMode.Create);
                await ProfileImage.CopyToAsync(stream);
                model.ProfileImagePath = "/uploads/images/" + ProfileImage.FileName;
            }

            _context.ServiceProviders.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("LaborSignin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
