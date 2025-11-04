using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Designation { get; set; }
    public string? Password { get; set; }
    public bool? Active { get; set; }
    public string? Email { get; set; }
    public int? GroupID { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public string? OutletIds { get; set; }
    public string? UserType { get; set; }
    public int? CompanyId { get; set; }
    public int? DefaultOutlet { get; set; }

    // 🔗 Navigation
    public Subscription? Subscription { get; set; }
    public GroupRoleMaster? GroupRole { get; set; }
}
