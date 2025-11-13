using MinimalApis.Entities.DTO;
using MinimalApis.Helpers;

namespace MinimalApis.RepositoryLayer.Interface;

public interface IOutletService
{
    Task<GenericResponse<IEnumerable<OutletDto>>> GetOutletsAsync();
    Task<GenericResponse<IQueryable<OutletDto>>> GetOutletsAsync2();
    Task<GenericResponse<bool>> AddOutletAsync(AddOutletDto model);
}
