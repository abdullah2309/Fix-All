using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fix_All.Controllers
{
    public class BookNowController : Controller
    {
        private readonly mydbcontext _context;

        public BookNowController(mydbcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Create(int laborId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "⚠️ Please login to continue.";
                return RedirectToAction("Signin", "Home");
            }

            var labor = _context.approve_labers
                .FirstOrDefault(l => l.ApproveLarberId == laborId && l.Status == "Approved");

            if (labor == null)
            {
                TempData["Error"] = "❌ Labor not found or not approved.";
                return RedirectToAction("Labers", "Home"); // User panel page
            }

            // Safe to access labor properties
            ViewBag.LaborStatus = labor.OnlineStatus;
            ViewBag.LaborName = labor.FirstName + " " + labor.LastName;
            ViewBag.Fields = _context.LaborFields.ToList();
            ViewBag.LaborId = labor.ApproveLarberId;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookNow booking)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "⚠️ You must be logged in to book a labor.";
                return RedirectToAction("Signin", "Home");
            }

            booking.UserId = userId.Value;
            booking.Status = "Pending";

            _context.BookNow.Add(booking);
            _context.SaveChanges();

            // Message bhej do Index page ke liye
            TempData["Message"] = "✅ Booking request sent successfully!";

            return RedirectToAction("Index", "Home"); // 👈 Index page par bhej do
        }


        //// Admin Approval
        public IActionResult Manage()
        {
            var email = HttpContext.Session.GetString("AdminEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login" , "admin");

            var bookings = _context.BookNow
                .Include(b => b.UserAccount)
                .Include(b => b.ApproveLaber)
                .Include(b => b.LaborField)
                .OrderByDescending(b => b.BookingId) // 🔹 agar latest pehle dikhana hai
                                                     //.OrderBy(b => b.BookingId)         // 🔹 agar ascending (1,2,3...) chahiye
                .ToList();

            return View(bookings);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var booking = _context.BookNow.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                TempData["Message"] = "Booking not found!";
                return RedirectToAction("Manage");
            }

            booking.Status = "Approved";
            _context.SaveChanges();

            TempData["Message"] = "Booking Approved Successfully!";
            return RedirectToAction("Manage");
        }


        [HttpPost]
        public IActionResult Reject(int id)
        {
            var booking = _context.BookNow.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                TempData["Message"] = "Booking not found!";
                return RedirectToAction("Manage");
            }

            booking.Status = "Rejected";
            _context.SaveChanges();

            TempData["Message"] = "Booking Rejected Successfully!";
            return RedirectToAction("Manage");
        }

    }
}
