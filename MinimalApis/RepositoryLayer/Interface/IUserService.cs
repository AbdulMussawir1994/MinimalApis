using MinimalApis.Entities.DTO;
using MinimalApis.Helpers;

namespace MinimalApis.RepositoryLayer.Interface;

public interface IUserService
{
    Task<GenericResponse<LoginResponseModelDto>> LoginUserAsync(LoginRequestDto model, CancellationToken cancellationToken);
    Task<GenericResponse<string>> RegisterUserAsync(RegisterViewModelDto model, CancellationToken cancellationToken);
    Task<GenericResponse<IQueryable<GetRolesByGroup>>> GetRolesByIdAsync(GetRolesById model);
    Task<GenericResponse<IEnumerable<OutletDto>>> GetOutletsAsync();
}
