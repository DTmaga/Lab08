using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using SchoolAppCoreMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new SingularControllerViewLocationExpander());
});
builder.Services.AddAuthorization();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDBConnectionString")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    var hasDepartmentsTable = db.Database
        .SqlQueryRaw<int>("SELECT CAST(CASE WHEN OBJECT_ID(N'dbo.Departments', N'U') IS NULL THEN 0 ELSE 1 END AS int) AS [Value]")
        .Single() == 1;

    if (!hasDepartmentsTable)
    {
        db.Database.Migrate();
    }
    else
    {
        db.Database.ExecuteSqlRaw("""
            IF COL_LENGTH(N'dbo.Departments', N'DepartmentName') IS NULL
            BEGIN
                ALTER TABLE [dbo].[Departments]
                ADD [DepartmentName] nvarchar(max) NOT NULL
                    CONSTRAINT [DF_Departments_DepartmentName] DEFAULT N'';
            END
            """);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

sealed class SingularControllerViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        var controller = context.ActionContext.RouteData.Values["controller"]?.ToString();
        if (string.IsNullOrEmpty(controller) || !controller.EndsWith('s'))
        {
            return viewLocations;
        }

        var singularController = controller[..^1];
        return viewLocations.Append($"/Views/{singularController}/{{0}}.cshtml");
    }
}
