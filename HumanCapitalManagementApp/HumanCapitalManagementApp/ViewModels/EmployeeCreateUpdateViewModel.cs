using HumanCapitalManagementApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagementApp.ViewModels
{
    public class EmployeeCreateUpdateViewModel
    {
        // Employee ID (nullable for create scenarios, required for update)
        public int? Id { get; set; }

        // Full name of the employee with validation
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        // Employee email with required validation and proper format
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // Password input, optional for update, required for creation
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }

        // Role selection for Identity (Employee, Manager, HR Admin)
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = null!;

        // Pre-defined list of roles for the dropdown selector
        public List<string> Roles { get; set; } = new() { "Employee", "Manager", "HR Admin" };

        // Department ID (required)
        [Display(Name = "Department")]
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        // Designation ID (required)
        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }

        // Hire Date with custom validation (not in the future)
        [Display(Name = "Hire Date")]
        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(EmployeeCreateUpdateViewModel), nameof(ValidateHireDate))]
        public DateTime HireDate { get; set; } = DateTime.Today;

        // Date of Birth with custom validation (between 18 and 60 years old)
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(EmployeeCreateUpdateViewModel), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-60);

        // Employee Type (e.g., Permanent, Contract, Intern)
        [Display(Name = "Employee Type")]
        [Required(ErrorMessage = "Employee Type is required")]
        public int EmployeeTypeId { get; set; }

        // Gender selection (Male, Female, Other), required field
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;

        // Salary field with positive value restriction
        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        // Lists for dropdown menus (Departments, Designations, EmployeeTypes)
        public List<Department>? Departments { get; set; }
        public List<Designation>? Designations { get; set; }
        public List<EmployeeType>? EmployeeTypes { get; set; }

        // Custom Validation Method for Hire Date (cannot be in the future)
        public static ValidationResult? ValidateHireDate(DateTime? hireDate, ValidationContext context)
        {
            if (!hireDate.HasValue)
                return new ValidationResult("Hire Date is required.");

            if (hireDate.Value.Date > DateTime.Today)
                return new ValidationResult("Hire Date cannot be in the future.");

            return ValidationResult.Success;
        }

        // Custom Validation Method for Date of Birth (between 18 and 60 years old)
        public static ValidationResult? ValidateDateOfBirth(DateTime? dob, ValidationContext context)
        {
            if (!dob.HasValue)
                return new ValidationResult("Date of Birth is required.");

            var minDob = DateTime.Today.AddYears(-60);
            var maxDob = DateTime.Today.AddYears(-18);

            if (dob.Value.Date < minDob || dob.Value.Date > maxDob)
                return new ValidationResult($"Date of Birth must be between {minDob:yyyy-MM-dd} and {maxDob:yyyy-MM-dd}.");

            return ValidationResult.Success;
        }
    }
}

