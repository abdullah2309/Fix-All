using Fix_All.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class LaborFieldsController : Controller
{
    private readonly mydbcontext _context;

    public LaborFieldsController(mydbcontext context)
    {
        _context = context;
    }

    // READ: List all .
    public async Task<IActionResult> Index()
    {
        return View(await _context.LaborFields.ToListAsync());
    }

   
    // CREATE: GET
    public IActionResult Create()
    {
        return View();
    }

    // CREATE: POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LaborField laborField)
    {
        if (ModelState.IsValid)
        {
            _context.Add(laborField);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(laborField);
    }

    // UPDATE: GET
    public async Task<IActionResult> Edit(int id)
    {
        var laborField = await _context.LaborFields.FindAsync(id);
        if (laborField == null) return NotFound();
        return View(laborField);
    }

    // UPDATE: POST ,
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LaborField laborField)
    {
        if (id != laborField.FieldId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(laborField);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LaborFields.Any(e => e.FieldId == id))
                    return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(laborField);
    }

    // DELETE: GET
    public async Task<IActionResult> Delete(int id)
    {
        var laborField = await _context.LaborFields.FindAsync(id);
        if (laborField == null) return NotFound();
        return View(laborField);
    }

    // DELETE: POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var laborField = await _context.LaborFields.FindAsync(id);
        if (laborField != null)
        {
            _context.LaborFields.Remove(laborField);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // DETAILS
    public async Task<IActionResult> Details(int id)
    {
        var laborField = await _context.LaborFields.FirstOrDefaultAsync(m => m.FieldId == id);
        if (laborField == null) return NotFound();
        return View(laborField);
    }
  

}
