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


        // GET
        [HttpGet]
        public IActionResult Create(int laborId)
        {
            var labor = _context.approve_labers
                .FirstOrDefault(l => l.ApproveLarberId == laborId && l.Status == "Approved");

            if (labor == null)
            {
                TempData["Error"] = "Labor not found or not approved.";
                return RedirectToAction("Labers", "Home"); // User panel page
            }

            // Safe to access labor properties now
            ViewBag.LaborStatus = labor.OnlineStatus;
            ViewBag.LaborName = labor.FirstName + " " + labor.LastName;
            ViewBag.Fields = _context.LaborFields.ToList();
            ViewBag.LaborId = labor.ApproveLarberId;

            return View();
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookNow booking)
        {
            // Get logged-in user ID from session (must be set at login)
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["Error"] = "You must be logged in to book a labor.";
                return RedirectToAction("SignIn", "Home");
            }

            // Assign user ID from session
            booking.UserId = userId.Value;

            // Ensure booking status is Pending
            booking.Status = "Pending";

            
                // Add booking to database
                _context.BookNow.Add(booking);
                _context.SaveChanges();

                TempData["Message"] = "Booking request sent successfully!";
                return RedirectToAction("Success");
            //}

            //// If validation fails, reload form with fields
            //ViewBag.Fields = _context.LaborFields.ToList();
            //return View(booking);
        }

        // Success page
        public IActionResult Success()
        {
            return View();
        }

        //// Admin Approval
        //public IActionResult Manage()
        //{
        //    var bookings = _context.BookNow
        //        .Include(b => b.UserAccount)
        //        .Include(b => b.ApproveLaber)
        //        .Include(b => b.LaborFields)
        //        .ToList();

        //    return View(bookings);
        //}

        public IActionResult Approve(int id)
        {
            var booking = _context.BookNow.Find(id);
            if (booking != null)
            {
                booking.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage");
        }

        public IActionResult Reject(int id)
        {
            var booking = _context.BookNow.Find(id);
            if (booking != null)
            {
                booking.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage");
        }
    }
}
