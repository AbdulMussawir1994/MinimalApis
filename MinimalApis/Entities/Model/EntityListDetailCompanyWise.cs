using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class EntityListDetailCompanyWise
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RowNo { get; set; }
    public string EntityCode { get; set; } = null!;
    public bool? Active { get; set; }
    public int? CompanyId { get; set; }

    // 🔗 Navigation
    public Subscription? Subscription { get; set; }
    public EntityList? Entity { get; set; }
}
