using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class GroupRoleDetail
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleDetailID { get; set; }
    public int? GroupID { get; set; }
    public string? EntityCode { get; set; }
    public bool? Allow { get; set; }
    public bool? New { get; set; }
    public bool? Edit { get; set; }

    // 🔗 Navigation
    public GroupRoleMaster? GroupRole { get; set; }
    public EntityList? Entity { get; set; }

    public ICollection<AppUser>? AppUsers { get; set; }
}
