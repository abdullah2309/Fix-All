using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fix_All.Controllers
{
    public class AdminController : Controller
    {
        private readonly mydbcontext _context;

        public AdminController(mydbcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
     

        public async Task<IActionResult> applyforlabar()
        {
            var providers = await _context.ServiceProviders.Include(s => s.LaborField) // related LaborField bhi laye
                .ToListAsync();

            return View(providers);
        }
    }
}
