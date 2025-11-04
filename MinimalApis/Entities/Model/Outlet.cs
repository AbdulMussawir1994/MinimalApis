using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Outlet
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string OutletName { get; set; } = null!;
    public int? CountryId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool? IsActive { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public int? CompanyId { get; set; }
    public string? OutletNumber { get; set; }
    public string? ArabicOutletName { get; set; }
    public string? ArabicAddress { get; set; }
    public string? ArabicArea { get; set; }
    public int? CurrencyID { get; set; }
    public int? OutletSequanceNo { get; set; }

    // 🔗 Navigation
    public Subscription? Subscription { get; set; }
    public Country? Country { get; set; }
    public Currency? Currency { get; set; }
    public ICollection<SubscriptionPaymentDetail>? PaymentDetails { get; set; }
}
