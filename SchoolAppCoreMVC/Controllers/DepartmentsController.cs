
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreMVC.Models;

public class DepartmentsController : Controller
{
    private readonly SchoolContext _context;

    public DepartmentsController(SchoolContext context)
    {
        _context = context;
    }

    // GET: DEPARTMENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Departments.ToListAsync());
    }

    // GET: DEPARTMENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _context.Departments
            .Include(d => d.Courses)
            .FirstOrDefaultAsync(m => m.DepartmentID == id);
        if (department == null)
        {
            return NotFound();
        }

        return View(department);
    }

    // GET: DEPARTMENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DEPARTMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DepartmentName,Budget,StartDate")] Department department)
    {
        if (ModelState.IsValid)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    // GET: DEPARTMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _context.Departments.FindAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return View(department);
    }

    // POST: DEPARTMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("DepartmentID,DepartmentName,Budget,StartDate")] Department department)
    {
        if (id != department.DepartmentID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.DepartmentID))
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
        return View(department);
    }

    // GET: DEPARTMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _context.Departments
            .FirstOrDefaultAsync(m => m.DepartmentID == id);
        if (department == null)
        {
            return NotFound();
        }

        return View(department);
    }

    // POST: DEPARTMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DepartmentExists(int id)
    {
        return _context.Departments.Any(e => e.DepartmentID == id);
    }
}
