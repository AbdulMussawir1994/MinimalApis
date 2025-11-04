using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Currency
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? Rate { get; set; }
    public bool? IsDefault { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencyImage { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public int? NumOfDecimal { get; set; }

    // 🔗 Navigation
    public ICollection<Country>? Countries { get; set; }
    public ICollection<Outlet>? Outlets { get; set; }
}