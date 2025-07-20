using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagementApp.ViewModels
{
    public class LoginViewModel
    {
        // Email address input with validation for required field and proper email format
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        // Password input with required validation and masked input via DataType.Password
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        // Optional checkbox for "Remember Me" functionality (persistent login)
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

