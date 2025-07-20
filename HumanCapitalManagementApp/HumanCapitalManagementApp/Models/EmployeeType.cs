using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagementApp.Models
{
    public class EmployeeType
    {
        // Primary key for EmployeeType entity
        [Key]
        public int Id { get; set; }

        // Name of the employee type (e.g., Permanent, Contract, Intern)
        [Required(ErrorMessage = "Employee Type name is required")]
        [StringLength(100, ErrorMessage = "Employee Type name cannot be longer than 100 characters")]
        public string Name { get; set; } = null!;

        // Status flag to determine if the employee type is active (e.g., used for soft-delete logic)
        [Required]
        public bool IsActive { get; set; }

        // Navigation property for the collection of employees associated with this employee type
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

