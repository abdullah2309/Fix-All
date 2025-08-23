using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Fix_All.Models;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fix_All.Controllers
{
    public class LaberPanelController : Controller
    {
        private readonly mydbcontext _context;
        private readonly IWebHostEnvironment _env;

        public LaberPanelController(mydbcontext context ,IWebHostEnvironment  env)
        {
            _context = context;
            _env = env;
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
        public IActionResult userpanelviewprofile()
        {
            return View();
        }
        // ---------------------------
        // View Profile
        // ---------------------------
        public IActionResult ViewProfile()
        {
          
            int? laborId = HttpContext.Session.GetInt32("LaberId");
            if (laborId == null)
            {
                return RedirectToAction("LaborSignin", "Account");
            }

            var labor = _context.approve_labers
                                .Include(l => l.LaborField)
                                .FirstOrDefault(l => l.ApproveLarberId == laborId.Value);

            if (labor == null) return NotFound();

            return View(labor);
        }

        // ---------------------------
        // Edit Profile (GET)
        // ---------------------------
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            var labor = _context.approve_labers.Find(id);
            if (labor == null) return NotFound();
            ViewBag.Fields = _context.LaborFields
       .Select(f => new SelectListItem
       {
           Value = f.FieldId.ToString(),
           Text = f.FieldName
       }).ToList();

            return View(labor);
        }

        // ---------------------------
        // Edit Profile (POST)
        // ---------------------------
        [HttpPost]
        public IActionResult EditProfile(approve_laber model, IFormFile? profileImage, IFormFile? coverImage, IFormFile? cv)
        {
            var labor = _context.approve_labers.FirstOrDefault(x => x.ApproveLarberId == model.ApproveLarberId);
            if (labor == null) return NotFound();

            // -----------------------
            // ✅ Manual validation
            // -----------------------
            if (string.IsNullOrWhiteSpace(model.FirstName))
                ModelState.AddModelError("FirstName", "First Name is required.");

            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("LastName", "Last Name is required.");

            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email is required.");
            else if (!model.Email.Contains("@"))
                ModelState.AddModelError("Email", "Invalid email format.");

            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Phone number is required.");

          
                // reload dropdown again
                ViewBag.Fields = _context.LaborFields
                    .Select(f => new SelectListItem
                    {
                        Value = f.FieldId.ToString(),
                        Text = f.FieldName
                    }).ToList();

           

            // -----------------------
            // ✅ Update fields
            // -----------------------
            labor.FirstName = model.FirstName;
            labor.LastName = model.LastName;
            labor.Email = model.Email;
            labor.Phone = model.Phone;
            labor.CNIC = model.CNIC;
            labor.Address = model.Address;
            labor.Education = model.Education;
            labor.FieldId = model.FieldId;
            labor.addmorefield = model.addmorefield;
            labor.Experience = model.Experience;
            labor.Status = model.Status;
            labor.Feedback = model.Feedback;
            labor.Headline = model.Headline;
            labor.About = model.About;
            labor.Skills = model.Skills;
            labor.FacebookUrl = model.FacebookUrl;

            // -----------------------
            // ✅ File Uploads
            // -----------------------
            string uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            // Profile
            if (profileImage != null)
            {
                if (!IsImageFile(profileImage))
                {
                    ModelState.AddModelError("ProfileImagePath", "Only JPG, PNG files allowed.");
                    return View(model);
                }

                string fileName = "profile_" + Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                string filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    profileImage.CopyTo(stream);

                labor.ProfileImagePath = "/uploads/" + fileName;
            }

            // Cover
            if (coverImage != null)
            {
                if (!IsImageFile(coverImage))
                {
                    ModelState.AddModelError("CoverImagePath", "Only JPG, PNG files allowed.");
                    return View(model);
                }

                string fileName = "cover_" + Guid.NewGuid() + Path.GetExtension(coverImage.FileName);
                string filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    coverImage.CopyTo(stream);

                labor.CoverImagePath = "/uploads/" + fileName;
            }

            // CV
            if (cv != null)
            {
                if (Path.GetExtension(cv.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("CVFilePath", "Only PDF files allowed.");
                    return View(model);
                }

                string fileName = "cv_" + Guid.NewGuid() + Path.GetExtension(cv.FileName);
                string filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    cv.CopyTo(stream);

                labor.CVFilePath = "/uploads/" + fileName;
            }

            _context.Update(labor);
            _context.SaveChanges();

            return RedirectToAction("ViewProfile", new { id = labor.ApproveLarberId });
        }


        // ---------------------------
        // Helper: Check file type
        // ---------------------------
        private bool IsImageFile(IFormFile file)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return permittedExtensions.Contains(ext);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("laber_panel_Login");
        }
    }
}
