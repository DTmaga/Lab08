using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.InstructorPages;

public class DeleteModel : PageModel
{
    private readonly SchoolContext _context;

    public DeleteModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Instructor Instructor { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? instructorid)
    {
        if (instructorid is null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.InstructorID == instructorid);
        if (instructor is null)
        {
            return NotFound();
        }
        else
        {
            Instructor = instructor;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? instructorid)
    {
        if (instructorid is null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors.FindAsync(instructorid);
        if (instructor != null)
        {
            Instructor = instructor;
            _context.Instructors.Remove(Instructor);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
