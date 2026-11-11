using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreMVC.Models;

namespace SchoolAppCoreMVC.Controllers;

public class StudentsController : Controller
{
    private readonly SchoolContext _context;

    public StudentsController(SchoolContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Students.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var student = await _context.Students.Include(s => s.Enrollments).AsNoTracking()
            .FirstOrDefaultAsync(s => s.StudentID == id);
        return student is null ? NotFound() : View(student);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,EnrollmentDate")] Student student)
    {
        if (!ModelState.IsValid) return View(student);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var student = await _context.Students.FindAsync(id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("StudentID,FirstName,LastName,EnrollmentDate")] Student student)
    {
        if (id != student.StudentID) return NotFound();
        if (!ModelState.IsValid) return View(student);
        try
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!StudentExists(student.StudentID))
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentID == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is not null) _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id) => _context.Students.Any(s => s.StudentID == id);
}
