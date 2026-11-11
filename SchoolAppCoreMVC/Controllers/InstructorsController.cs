
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreMVC.Models;

public class InstructorsController : Controller
{
    private readonly SchoolContext _context;

    public InstructorsController(SchoolContext context)
    {
        _context = context;
    }

    // GET: INSTRUCTORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Instructors.ToListAsync());
    }

    // GET: INSTRUCTORS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(m => m.InstructorID == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // GET: INSTRUCTORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: INSTRUCTORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("InstructorID,LastName,FirstName,HireDate")] Instructor instructor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(instructor);
    }

    // GET: INSTRUCTORS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor == null)
        {
            return NotFound();
        }
        return View(instructor);
    }

    // POST: INSTRUCTORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("InstructorID,LastName,FirstName,HireDate")] Instructor instructor)
    {
        if (id != instructor.InstructorID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(instructor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(instructor.InstructorID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(instructor);
    }

    // GET: INSTRUCTORS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(m => m.InstructorID == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    // POST: INSTRUCTORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool InstructorExists(int id)
    {
        return _context.Instructors.Any(e => e.InstructorID == id);
    }
}
