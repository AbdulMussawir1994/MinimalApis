using MinimalApis.Entities.DTO;
using MinimalApis.Helpers;

namespace MinimalApis.RepositoryLayer.Interface;

public interface IExpenseService
{
    Task<GenericResponse<bool>> AddExpenseAsync(CreateExpenseDto model, CancellationToken token);
}
