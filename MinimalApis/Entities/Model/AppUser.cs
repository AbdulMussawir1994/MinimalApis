using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class AppUser : IdentityUser
{
    public long? CreatedBy { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public int GroupId { get; set; }

    public long OutletsId { get; set; }

    [Required]
    public int GroupRoleGroupId { get; set; }

    // Navigation properties
    public virtual Subscription Company { get; set; } = null!;
    public virtual GroupRoleMaster Group { get; set; } = null!;
    public virtual GroupRoleDetail GroupRoleId { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}