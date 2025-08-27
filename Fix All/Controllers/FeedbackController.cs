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
            // Save feedback
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            // ✅ Find related booking
            var booking = _context.BookNow.FirstOrDefault(b => b.BookingId == feedback.BookingId);
            if (booking != null)
            {
                booking.Status = "Done";  // update booking status
                _context.BookNow.Update(booking);
                _context.SaveChanges();
            }

            TempData["Success"] = "Thanks for your feedback!";
            return RedirectToAction("Index", "Home"); // redirect to customer panel

        }
    }
}
