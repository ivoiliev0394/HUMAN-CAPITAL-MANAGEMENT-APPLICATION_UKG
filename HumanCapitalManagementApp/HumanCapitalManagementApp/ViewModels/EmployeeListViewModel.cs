using HumanCapitalManagementApp.Models;
using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagementApp.ViewModels
{
    public class EmployeeListViewModel
    {
        // List of employees to be displayed on the current page
        public List<Employee>? Employees { get; set; }

        // Total number of pages available (used for pagination controls)
        [Display(Name = "Total Pages")]
        public int TotalPages { get; set; }

        // Search term entered by the user for filtering employee names
        [Display(Name = "Search Term")]
        public string? SearchTerm { get; set; }

        // Currently selected Department filter (nullable)
        [Display(Name = "Department")]
        public int? SelectedDepartmentId { get; set; }

        // Currently selected Employee Type filter (nullable)
        [Display(Name = "Employee Type")]
        public int? SelectedEmployeeTypeId { get; set; }

        // Number of employees to display per page with validation for acceptable range
        [Display(Name = "Page Size")]
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; }

        // Current page number with validation to avoid negative/zero values
        [Display(Name = "Page Number")]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int PageNumber { get; set; }

        // Total count of employees matching current filter (without pagination)
        [Display(Name = "Total Employees")]
        public int Total { get; set; }

        // List of all departments (for filter dropdown)
        public List<Department>? Departments { get; set; }

        // List of all employee types (for filter dropdown)
        public List<EmployeeType>? EmployeeTypes { get; set; }
    }
}
