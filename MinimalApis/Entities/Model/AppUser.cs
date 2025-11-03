using Microsoft.AspNetCore.Identity;

namespace MinimalApis.Entities.Model;

public class AppUser : IdentityUser
{
    public long? CreatedBy { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}