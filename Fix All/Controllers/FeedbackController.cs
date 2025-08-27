using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fix_All.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly mydbcontext _context;

        public FeedbackController(mydbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int bookingId)
        {
            ViewBag.BookingId = bookingId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                TempData["Success"] = "Thanks for your feedback!";
                return RedirectToAction("MyBookings", "UserPanel"); // redirect to customer panel
            }

            return View(feedback);
        }
    }
}
