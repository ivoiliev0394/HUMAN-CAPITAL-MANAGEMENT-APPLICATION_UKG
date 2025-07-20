using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanCapitalManagementApp.Models
{
    public class Employee
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Full name of the employee with validation for required field and max length
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters")]
        public string FullName { get; set; } = null!;

        // Employee email with required and email address validation
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        // Password used during initial creation, typically synced with IdentityUser
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters")]
        public string Password { get; set; } = null!;

        // Foreign Key to ASP.NET Identity user (optional, nullable for flexibility)
        public string? IdentityUserId { get; set; }

        // Navigation property to IdentityUser account (login credentials)
        [ForeignKey("IdentityUserId")]
        public IdentityUser? IdentityUser { get; set; }

        // Foreign Key to Department
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        // Navigation property to related Department
        public Department Department { get; set; } = null!;

        // Foreign Key to Designation
        [Required(ErrorMessage = "Designation is required")]
        public int DesignationId { get; set; }

        // Navigation property to related Designation
        public Designation Designation { get; set; } = null!;

        // Date of hire
        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        // Date of birth
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // Foreign Key to EmployeeType (Permanent, Contract, etc.)
        [Required(ErrorMessage = "Employee Type is required")]
        public int EmployeeTypeId { get; set; }

        // Navigation property to EmployeeType
        public EmployeeType EmployeeType { get; set; } = null!;

        // Gender of the employee with validation constraint
        [Required(ErrorMessage = "Gender is required")]
        [StringLength(6, ErrorMessage = "Gender should be Male, Female, or Other")]
        public string Gender { get; set; } = null!;

        // Salary with validation for positive numbers and SQL column precision setup
        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
    }
}

