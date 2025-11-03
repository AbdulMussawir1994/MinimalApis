using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Employee
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = default!;

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual AppUser User { get; set; } = null!;
}