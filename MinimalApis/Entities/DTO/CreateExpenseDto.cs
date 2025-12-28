using MinimalApis.Enums;
using System.ComponentModel.DataAnnotations;

namespace MinimalApis.Entities.DTO;

public readonly record struct CreateExpenseDto
{
    public string Title { get; init; }
    public decimal amount { get; init; }
    public string description { get; init; }
    public DateTime DueDate { get; init; }

    [EnumDataType(typeof(ExpenseCategory), ErrorMessage = "Invalid Category")]
    public string Category { get; init; }

    [EnumDataType(typeof(ExpenseType), ErrorMessage = "Invalid Type")]
    public string Type { get; init; }
}
