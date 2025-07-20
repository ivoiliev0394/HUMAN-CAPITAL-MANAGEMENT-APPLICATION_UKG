using HumanCapitalManagementApp.Models;
using HumanCapitalManagementApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HumanCapitalManagementApp.Controllers
{
    public class HomeController : Controller
    {
        // Logger service for logging information, warnings, errors etc.
        private readonly ILogger<HomeController> _logger;

        // Constructor with dependency injection for ILogger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        // Returns the home page view
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        // Returns the privacy policy view
        public IActionResult Privacy()
        {
            return View();
        }

        // Handles errors and returns the error page view with error details
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Creating ErrorViewModel with RequestId for tracking purposes
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
