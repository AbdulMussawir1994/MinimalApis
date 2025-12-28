using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.Helpers;
using MinimalApis.RepositoryLayer.Interface;

namespace MinimalApis.RepositoryLayer.Service;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _db;

    public ExpenseService(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<GenericResponse<bool>> AddExpenseAsync(CreateExpenseDto dto, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            ExpenseName = dto.Title,
            Amount = dto.amount,
            Description = dto.description,
            DueDate = dto.DueDate,
            Category = dto.Category,
            Type = dto.Type,
            IsActive = true
        };

        _db.Expenses.Add(expense);

        if (await _db.SaveChangesAsync(cancellationToken) == 0)
        {
            return GenericResponse<bool>.Fail(
                "Failed to add expense",
                StatusCodes.Status400BadRequest.ToString());
        }

        return GenericResponse<bool>.Success(
            true,
            "Expense added successfully",
            StatusCodes.Status201Created.ToString());
    }
}
