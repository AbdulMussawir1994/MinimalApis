using MinimalApis.Entities.DTO;
using MinimalApis.Helpers;

namespace MinimalApis.RepositoryLayer.Interface;

public interface IUserService
{
    Task<GenericsResponse<LoginResponseModelDto>> LoginUserAsync(LoginRequestDto model, CancellationToken cancellationToken);
    Task<GenericsResponse<string>> RegisterUserAsync(RegisterViewModelDto model, CancellationToken cancellationToken);
}
