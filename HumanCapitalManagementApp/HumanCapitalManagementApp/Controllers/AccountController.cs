using HumanCapitalManagementApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagementApp.Controllers
{
    public class AccountController : Controller
    {
        // SignInManager service from ASP.NET Identity used to handle login and logout processes
        private readonly SignInManager<IdentityUser> _signInManager;

        // Constructor with dependency injection for SignInManager
        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        // Returns the login view when a GET request is made
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        // Handles the login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // If the model is invalid (e.g., required fields are missing), return the same view with validation errors
            if (!ModelState.IsValid) return View(model);

            // Attempts to sign in the user with the provided email, password, and remember me option
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // If login is successful, redirect to the Home page
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            // If login fails, show a generic error message
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        // GET: /Account/AccessDenied
        // Returns the Access Denied view when the user tries to access a restricted resource
        public IActionResult AccessDenied()
        {
            return View();
        }

        // POST: /Account/Logout
        // Signs out the currently logged-in user and redirects to the Login page
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

