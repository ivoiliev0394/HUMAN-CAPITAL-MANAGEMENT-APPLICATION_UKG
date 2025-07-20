using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanCapitalManagementApp.Models
{
    public class Designation
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Name of the designation (e.g., Software Developer, HR Specialist)
        [Required(ErrorMessage = "Designation name is required")]
        [StringLength(100, ErrorMessage = "Designation name cannot be longer than 100 characters")]
        public string Name { get; set; } = null!;

        // Foreign Key to Department
        [Required]
        public int DepartmentId { get; set; }

        // Navigation property to the related Department entity
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;

        // Indicates whether the designation is active (status flag)
        [Required]
        public bool IsActive { get; set; }
    }
}

