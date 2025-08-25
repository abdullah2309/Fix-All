using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

using ServiceProviderModel = Fix_All.Models.ServiceProvider;

namespace Fix_All.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly mydbcontext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            mydbcontext context,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _env = env;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var labors = _context.approve_labers.ToList();
            return View(labors);
        }
        public IActionResult Labers(int? fieldId, string searchTerm)
        {
            var onlineLabors = _context.approve_labers
                .Where(l => l.Status == "Approved" && l.OnlineStatus == "Online")
                .AsQueryable();

            var offlineLabors = _context.approve_labers
                .Where(l => l.Status == "Approved" && l.OnlineStatus == "Offline")
                .AsQueryable();

            // ✅ Filter by Category
            if (fieldId.HasValue && fieldId.Value > 0)
            {
                onlineLabors = onlineLabors.Where(l => l.FieldId == fieldId.Value);
                offlineLabors = offlineLabors.Where(l => l.FieldId == fieldId.Value);
            }

            // ✅ Search by Name, Headline, Skills, AddMoreField
            if (!string.IsNullOrEmpty(searchTerm))
            {
                onlineLabors = onlineLabors.Where(l =>
                    l.FirstName.Contains(searchTerm) ||
                    l.LastName.Contains(searchTerm) ||
                    l.Headline.Contains(searchTerm) ||
                    l.Skills.Contains(searchTerm) ||                // 👈 skills search
                    l.addmorefield.Contains(searchTerm)             // 👈 addmorefield search
                );

                offlineLabors = offlineLabors.Where(l =>
                    l.FirstName.Contains(searchTerm) ||
                    l.LastName.Contains(searchTerm) ||
                    l.Headline.Contains(searchTerm) ||
                    l.Skills.Contains(searchTerm) ||
                    l.addmorefield.Contains(searchTerm)
                );
            }

            // ✅ Pass data to View
            ViewBag.Fields = _context.LaborFields.ToList();
            ViewBag.SelectedFieldId = fieldId ?? 0;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.OfflineLabors = offlineLabors.ToList();

            return View(onlineLabors.ToList());
        }


        public IActionResult About() => View();
        public IActionResult Services() => View();
        public IActionResult Contact() => View();
        [HttpPost]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact");
            }

            return View(model);
        }

        public IActionResult Signin() => View();
        [HttpPost]
        public IActionResult SignUp(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                _context.UserAccounts.Add(model);
                _context.SaveChanges();
                TempData["Message"] = "Account created successfully!";
                return RedirectToAction("SignIn");
            }
            TempData["Error"] = "Something went wrong!";
            return RedirectToAction("SignUp");
        }

        [HttpPost]
        public IActionResult SignIn(UserAccount model)
        {
            var user = _context.UserAccounts
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                // ✅ Session set karna
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetInt32("UserId", user.Id);

                
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid Email or Password!";
            return RedirectToAction("SignIn");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // ✅ sab session clear
            return RedirectToAction("index", "Home");
        }


        public IActionResult applynow() => View();
        public IActionResult LaborProfile(int id)
        {
            var labor = _context.approve_labers
                                .FirstOrDefault(l => l.ApproveLarberId == id);

            if (labor == null)
            {
                return NotFound();
            }

            return View(labor);
        }

        [HttpGet]
        public IActionResult LaborSignin()
        {
            PopulateFieldsDropdown();
            return View(new ServiceProviderModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LaborSignin(ServiceProviderModel model, IFormFile? cv, IFormFile? profileImage, string confirmPassword)
        {
            // ✅ Confirm password check
            if (model.PasswordHash != confirmPassword)
            {
                ModelState.AddModelError("PasswordHash", "Passwords do not match!");
            }

            // ✅ Repopulate dropdown if validation fails
            PopulateFieldsDropdown();

            // ✅ Handle CV upload OR keep old one
            if (cv != null && cv.Length > 0)
            {
                model.CVFilePath = SaveFile(cv, "uploads/cv");
            }
            else if (!string.IsNullOrEmpty(model.CVFilePath))
            {
                ModelState.Remove("CVFilePath"); // keep existing
            }

            // ✅ Handle Profile upload OR keep old one
            if (profileImage != null && profileImage.Length > 0)
            {
                model.ProfileImagePath = SaveFile(profileImage, "uploads/profile");
            }
            else if (!string.IsNullOrEmpty(model.ProfileImagePath))
            {
                ModelState.Remove("ProfileImagePath"); // keep existing
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ✅ Hash password before saving
            model.PasswordHash = HashPassword(model.PasswordHash);

            // ✅ Save to DB
            _context.ServiceProviders.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private string SaveFile(IFormFile file, string folderPath)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var uploadPath = Path.Combine(_env.WebRootPath, folderPath);
            Directory.CreateDirectory(uploadPath); // ensure folder exists
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return "/" + folderPath + "/" + fileName;
        }

        private void PopulateFieldsDropdown()
        {
            ViewBag.Fields = _context.LaborFields
                .Select(f => new SelectListItem
                {
                    Value = f.FieldId.ToString(),
                    Text = f.FieldName
                }).ToList();
        }
        public IActionResult _LaborProfileCard()
        {
            return View();
        }
        public IActionResult _LaborProfileCard2()
        {
            return View();
        }





    }
}
