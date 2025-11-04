using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class EntityList
{
    [Key]
    [Column(TypeName = "varchar(25)")]
    public string EntityCode { get; set; } = null!;
    public string? ModuleCode { get; set; }
    public string? EntityName { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RowNo { get; set; }
    public string? Path { get; set; }
    public int? OrderNum { get; set; }
    public bool? Active { get; set; }
    public string? Icon { get; set; }
    public bool IsParent { get; set; }

    // 🔗 Navigation
    public ICollection<GroupRoleDetail>? GroupRoleDetails { get; set; }
    public ICollection<EntityListDetailCompanyWise>? CompanyWiseDetails { get; set; }
}
