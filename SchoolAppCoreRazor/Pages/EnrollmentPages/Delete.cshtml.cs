using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.EnrollmentPages;

public class DeleteModel : PageModel
{
    private readonly SchoolContext _context;

    public DeleteModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Enrollment Enrollment { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? enrollmentid)
    {
        if (enrollmentid is null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments.FirstOrDefaultAsync(m => m.EnrollmentID == enrollmentid);
        if (enrollment is null)
        {
            return NotFound();
        }
        else
        {
            Enrollment = enrollment;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? enrollmentid)
    {
        if (enrollmentid is null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments.FindAsync(enrollmentid);
        if (enrollment != null)
        {
            Enrollment = enrollment;
            _context.Enrollments.Remove(Enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
