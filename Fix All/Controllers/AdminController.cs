using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fix_All.Controllers
{
    public class AdminController : Controller
    {
        private readonly mydbcontext mydbcontext;

        public AdminController(mydbcontext mydbcontext)
        {
            this.mydbcontext = mydbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add_Labor_Category()
        {
            ViewData["Show_Add_Labor_Category"] = mydbcontext.add_Labor_Categories;
            return View();
        }
        [HttpPost]
        public IActionResult Add_Labor_Category(Add_labor_Category add_Labor_Category)
        {
            if (ModelState.IsValid)
            {
                mydbcontext.add_Labor_Categories.Add(add_Labor_Category);
                mydbcontext.SaveChanges();
                return RedirectToAction("Add_Labor_Category");
            }
            return RedirectToAction(nameof(Add_Labor_Category));
        }
        public IActionResult _show_Add_Labor_Category()
        {
            var categories = mydbcontext.add_Labor_Categories.ToList();
            return View(categories);
        }

        public IActionResult Table()
        {
            return View();
        }
    }
}
