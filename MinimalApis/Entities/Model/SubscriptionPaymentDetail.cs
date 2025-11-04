using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class SubscriptionPaymentDetail
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubPaymentDetailID { get; set; }

    public int? CompanyID { get; set; }
    public int? OutletID { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public decimal? Fee { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }

    // 🔗 Navigation
    public Subscription? Subscription { get; set; }
    public Outlet? Outlet { get; set; }
}
