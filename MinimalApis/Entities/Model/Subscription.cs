using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Subscription
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyID { get; set; }
    public string? CompanyName { get; set; }
    public string? OutletsCount { get; set; }
    public int? CountryID { get; set; }

    public string? Mobile { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifyBy { get; set; }
    public DateTime? ModifyOn { get; set; }
    public string? CountryCode { get; set; }
    public string? ContactPerson { get; set; }
    public string? CompanyURLName { get; set; }
    public int? PricePlan { get; set; }
    public bool? IsActive { get; set; }

    // 🔗 Relationships
    public Country? Country { get; set; }
    public ICollection<Outlet>? Outlets { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<GroupRoleMaster>? GroupRoles { get; set; }
    public ICollection<SubscriptionPaymentDetail>? Payments { get; set; }
    public ICollection<EntityListDetailCompanyWise>? EntityAccess { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
