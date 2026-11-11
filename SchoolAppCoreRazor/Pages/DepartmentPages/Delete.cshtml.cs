using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.DepartmentPages;

public class DeleteModel : PageModel
{
    private readonly SchoolContext _context;

    public DeleteModel(SchoolContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Department Department { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? departmentid)
    {
        if (departmentid is null)
        {
            return NotFound();
        }

        var department = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == departmentid);
        if (department is null)
        {
            return NotFound();
        }
        else
        {
            Department = department;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? departmentid)
    {
        if (departmentid is null)
        {
            return NotFound();
        }

        var department = await _context.Departments.FindAsync(departmentid);
        if (department != null)
        {
            Department = department;
            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
