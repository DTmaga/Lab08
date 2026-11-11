using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCoreRazor.Models;

namespace SchoolAppCoreRazor.Pages.CoursePages;

public class CreateModel : PageModel
{
    private readonly SchoolContext _context;

    public CreateModel(SchoolContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        await LoadDepartmentsAsync();
    }

    [BindProperty]
    public Course Course { get; set; } = default!;

    public SelectList Departments { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync();
            return Page();
        }

        _context.Courses.Add(Course);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private async Task LoadDepartmentsAsync()
    {
        Departments = new SelectList(
            await _context.Departments.AsNoTracking().OrderBy(d => d.DepartmentName).ToListAsync(),
            nameof(Department.DepartmentID),
            nameof(Department.DepartmentName));
    }
}
