using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Country
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CountryID { get; set; }
    public string? CountryName { get; set; }
    public string? ACountryName { get; set; }
    public bool? Active { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public string? CountryCode { get; set; }
    public int? CurrencyId { get; set; }

    // 🔗 Navigation
    public Currency? Currency { get; set; }
    public ICollection<Subscription>? Subscriptions { get; set; }
    public ICollection<Outlet>? Outlets { get; set; }
}
