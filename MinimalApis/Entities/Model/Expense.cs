using MinimalApis.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Expense
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string ExpenseName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }

    [EnumDataType(typeof(ExpenseCategory), ErrorMessage = "Invalid Category")]
    // [RegularExpression("C|A|E", ErrorMessage = "Invalid Type, Allowed Types C|A|E")]
    public string Category { get; set; } = string.Empty;

    [EnumDataType(typeof(ExpenseType), ErrorMessage = "Invalid Type")]
    public string Type { get; set; } = string.Empty;
}
