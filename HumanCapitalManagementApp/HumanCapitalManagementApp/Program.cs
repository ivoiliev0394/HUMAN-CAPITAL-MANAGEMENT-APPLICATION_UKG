using HumanCapitalManagementApp.Data;
using HumanCapitalManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Register services into the DI container =====

// Add MVC controllers and views support
builder.Services.AddControllersWithViews();

// Register Entity Framework Core with SQL Server connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom EmployeeService as Scoped (per HTTP request)
builder.Services.AddScoped<EmployeeService>();

// Configure ASP.NET Core Identity for user authentication and authorization
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>() // Use EF Core for Identity stores
    .AddDefaultTokenProviders(); // Enable token generation for password reset etc.

// Add authentication and authorization services
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// ===== Build the application =====
var app = builder.Build();

// ===== Seed Roles and User Accounts =====
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Define application roles
    string[] roles = { "Employee", "Manager", "HR Admin" };

    // Create roles if they do not exist
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed user accounts based on existing Employees without Identity account
    var employees = db.Employees.Where(e => e.IdentityUserId == null).ToList();

    foreach (var emp in employees)
    {
        // Skip if user with this email already exists
        var existingUser = await userManager.FindByEmailAsync(emp.Email);
        if (existingUser != null) continue;

        // Create IdentityUser for each Employee
        var user = new IdentityUser
        {
            UserName = emp.Email,
            Email = emp.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, emp.Password!);

        if (result.Succeeded)
        {
            // Link IdentityUserId to Employee record
            emp.IdentityUserId = user.Id;

            // Assign role based on Employee ID (example logic)
            string role = emp.Id switch
            {
                1 or 2 => "HR Admin",
                3 or 4 or 5 or 6 => "Manager",
                _ => "Employee"
            };

            await userManager.AddToRoleAsync(user, role);
        }
    }

    // Save changes to database after assigning accounts and roles
    await db.SaveChangesAsync();
}

// ===== Middleware Pipeline =====

// Use error handling in Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enforce HTTPS Strict Transport Security
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles();      // Enable serving static files (CSS, JS, images)

app.UseRouting();          // Enable request routing
app.UseAuthentication();   // Enable Identity authentication
app.UseAuthorization();    // Enable authorization based on roles

// Define the default route pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application
app.Run();
