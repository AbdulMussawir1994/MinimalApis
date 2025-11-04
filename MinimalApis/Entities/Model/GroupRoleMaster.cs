using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class GroupRoleMaster
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupID { get; set; }
    public string? GroupName { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public int? CompanyId { get; set; }

    // 🔗 Relationships
    public Subscription? Subscription { get; set; }
    public ICollection<GroupRoleDetail>? RoleDetails { get; set; }
    public ICollection<AppUser>? AppUsers { get; set; }
}
