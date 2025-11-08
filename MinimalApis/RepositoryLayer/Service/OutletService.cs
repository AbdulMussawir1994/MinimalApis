using Microsoft.EntityFrameworkCore;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.Helpers;
using MinimalApis.RepositoryLayer.Interface;

namespace MinimalApis.RepositoryLayer.Service
{
    public class OutletService : IOutletService
    {
        private readonly AppDbContext _db;
        public OutletService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<IEnumerable<OutletDto>>> GetOutletsAsync()
        {
            var outlets = await _db.Outlets
                .AsNoTracking()
                .Where(o => o.IsActive ?? false)
                .OrderBy(o => o.OutletName)
                .Select(o => new OutletDto(
                    o.OutletName,
                    o.Address ?? string.Empty,
                    o.IsActive ?? false,
                    false,   // Delivery
                    true,    // Pickup
                    false    // Dine
                ))
                .ToListAsync();

            if (!outlets.Any())
            {
                return GenericResponse<IEnumerable<OutletDto>>.Fail(
                    "No outlets found.",
                    "ERROR-404"
                );
            }

            return GenericResponse<IEnumerable<OutletDto>>.Success(
                outlets,
                "Outlets fetched successfully.",
                "SUCCESS-200"
            );
        }

        public async Task<GenericResponse<IQueryable<OutletDto>>> GetOutletsAsync2()
        {
            IQueryable<OutletDto> outletsQuery = _db.Outlets
                .AsNoTracking()
                .Where(o => o.IsActive ?? false)
                .OrderBy(o => o.OutletName)
                .Select(o => new OutletDto(
                    o.OutletName,
                    o.Address ?? string.Empty,
                    o.IsActive ?? false,
                    false,
                    true,
                    false
                ));

            bool hasData = await outletsQuery.AnyAsync();

            if (!hasData)
            {
                return GenericResponse<IQueryable<OutletDto>>.Fail(
                    "No active outlets found.",
                    "ERROR-404"
                );
            }

            return GenericResponse<IQueryable<OutletDto>>.Success(
                outletsQuery,
                "Outlets fetched successfully.",
                "SUCCESS-200"
            );
        }

        public async Task<GenericResponse<bool>> AddOutletAsync(AddOutletDto model)
        {
            bool exists = await _db.Outlets
                .AsNoTracking()
                .AnyAsync(x => x.OutletName == model.Name && x.Email == model.Email && x.CompanyId == model.CompanyId && x.CountryId == model.CountryId);

            if (exists)
            {
                return GenericResponse<bool>.Fail(
                    $"An outlet with the name '{model.Name}' already exists for the given company and country.",
                    "ERROR-409"
                );
            }

            var outlet = new Outlet
            {
                OutletName = model.Name.Trim(),
                CountryId = model.CountryId,
                Email = model.Email?.Trim(),
                Phone = model.Phone?.Trim(),
                Address = model.Address?.Trim(),
                IsActive = model.Active,
                CreatedBy = "1", // Ideally, pass from the user context or token
                CreatedOn = DateTime.UtcNow,
                CompanyId = model.CompanyId,
                CurrencyID = model.CurrencyId
            };

            await _db.Outlets.AddAsync(outlet).ConfigureAwait(false);

            var affected = await _db.SaveChangesAsync().ConfigureAwait(false);

            return affected > 0
                ? GenericResponse<bool>.Success(data: true, message: "New outlet created successfully.", code: "SUCCESS-200")
                : GenericResponse<bool>.Fail(message: "Failed to create outlet.", code: "ERROR-500");
        }
    }
}
