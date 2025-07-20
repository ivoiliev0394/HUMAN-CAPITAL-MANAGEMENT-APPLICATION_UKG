using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagementApp.Models
{
    public class Department
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Department name with validation rules
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot be longer than 100 characters")]
        public string Name { get; set; } = null!;

        // Boolean flag to indicate if the department is active (soft-delete or status flag)
        [Required]
        public bool IsActive { get; set; }

        // Navigation property for related Designations (one department can have many designations)
        public virtual ICollection<Designation> Designations { get; set; } = new List<Designation>();

        // Navigation property for related Employees (one department can have many employees)
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

