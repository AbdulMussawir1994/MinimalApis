using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.Helpers;
using MinimalApis.RepositoryLayer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApis.RepositoryLayer.Service;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _db;
    private readonly SymmetricSecurityKey _cachedKey;

    public UserService(UserManager<AppUser> userManager, IConfiguration configuration, AppDbContext DbContext)
    {
        _userManager = userManager;
        _configuration = configuration;
        _db = DbContext;

        var secret = _configuration["JWTKey:Secret"] ?? throw new InvalidOperationException("JWT secret is missing.");
        _cachedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }

    public async Task<GenericResponse<LoginResponseModelDto>> LoginUserAsync(LoginRequestDto model, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(model.userName).ConfigureAwait(false);

        if (user is null)
            return GenericResponse<LoginResponseModelDto>.Fail("Invalid username.", "Error-404");

        if (await _userManager.IsLockedOutAsync(user).ConfigureAwait(false))
            return GenericResponse<LoginResponseModelDto>.Fail("Account locked. Try again after 60 minutes.", "Error-400");

        if (!await _userManager.CheckPasswordAsync(user, model.password).ConfigureAwait(false))
        {
            await _userManager.AccessFailedAsync(user).ConfigureAwait(false);

            if (await _userManager.IsLockedOutAsync(user).ConfigureAwait(false))
                return GenericResponse<LoginResponseModelDto>.Fail("Account locked. Try again after 60 minutes.", "Error-400");

            return GenericResponse<LoginResponseModelDto>.Fail("Invalid password.", "Error-404");
        }

        await _userManager.ResetAccessFailedCountAsync(user).ConfigureAwait(false);

        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        if (roles is null || !roles.Any())
            roles = new List<string> { "User" };

        //    var token = GenerateKmacJwtToken(user.Id, roles, user.Email);
        var token = GenerateToken(user.Id, roles, user.Email);

        return GenericResponse<LoginResponseModelDto>.Success(
            new LoginResponseModelDto { token = token },
            "Login successful.",
            "SUCCESS-200"
        );
    }

    public async Task<GenericResponse<string>> RegisterUserAsync(RegisterViewModelDto model, CancellationToken cancellationToken)
    {

        var checkEmail = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);

        if (checkEmail is not null)
            return GenericResponse<string>.Fail("Email is already registered.", "Error-400");

        var checkUsername = await _userManager.FindByNameAsync(model.Email).ConfigureAwait(false);

        if (checkUsername is not null)
            return GenericResponse<string>.Fail("Username is already registered.", "Error-400");


        var user = new AppUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            DateCreated = DateTime.UtcNow,
            LockoutEnabled = true,
            GroupId = 1,
            CompanyId = 1,
        };

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                return GenericResponse<string>.ExceptionFailed(result.Errors.FirstOrDefault()?.Description ?? "Unknown error.", "REGISTRATION_FAILED", "Error-400");
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return GenericResponse<string>.Success(model.Username, "Register successful.", "SUCCESS-200");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            return GenericResponse<string>.ExceptionFailed(ex.Message, "REGISTRATION_FAILED", "Error-500");
        }
    }

    public async Task<GenericResponse<IQueryable<GetRolesByGroup>>> GetRolesByIdAsync(GetRolesById model)
    {
        bool userExists = await _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == model.UserId && u.IsActive);

        if (!userExists)
        {
            return GenericResponse<IQueryable<GetRolesByGroup>>.Fail(
                message: "User not found or inactive.",
                code: "ERROR-404"
            );
        }

        IQueryable<GetRolesByGroup> query =
            from u in _db.Users.AsNoTracking()
            join s in _db.Subscriptions.AsNoTracking() on u.CompanyId equals s.CompanyID
            join gr in _db.GroupRolesMaster.AsNoTracking()
                on new { u.GroupId, CompanyId = (int?)u.CompanyId }
                equals new { GroupId = gr.GroupID, CompanyId = gr.CompanyId }
            join gd in _db.GroupRolesDetails.AsNoTracking()
                on gr.GroupID equals gd.GroupID
            join e in _db.EntityLists.AsNoTracking()
                on gd.EntityCode equals e.EntityCode
            where u.Id == model.UserId
                  && u.IsActive
                  && (s.IsActive ?? false)
                  && (e.Active ?? false)
            select new GetRolesByGroup
            {
                RoleDetailId = gd.RoleDetailID,
                EntityCode = gd.EntityCode,
                Allow = gd.Allow ?? false,
                New = gd.New ?? false,
                Edit = gd.Edit ?? false,
                Path = e.Path,
                OrderNum = e.OrderNum,
                Icon = e.Icon
            };

        return GenericResponse<IQueryable<GetRolesByGroup>>.Success(
            data: query,
            message: "Roles fetched successfully.",
            code: "SUCCESS-200"
        );
    }

    private string GenerateToken(string userId, IEnumerable<string> roles, string email)
    {
        var utcNow = DateTime.UtcNow;

        // ✅ Core JWT claims
        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, userId),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new(ClaimTypes.NameIdentifier, userId),
        new(ClaimTypes.Email, email)
    };

        // ✅ Add role claims efficiently
        if (roles is not null)
        {
            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var secret = _configuration["JWTKey:Secret"]
             ?? throw new InvalidOperationException("JWT secret is missing.");

        var _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // ✅ Use HMAC-SHA-512 (stronger than SHA-256)
        var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha512);

        // ✅ Build token directly — faster than descriptor conversion
        var token = new JwtSecurityToken(
            issuer: _configuration["JWTKey:ValidIssuer"],
            audience: _configuration["JWTKey:ValidAudience"],
            claims: claims,
            notBefore: utcNow,
            expires: utcNow.AddMinutes(int.Parse(_configuration["JWTKey:TokenExpiryTimeInMinutes"] ?? "30")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateKmacJwtToken(string userId, IEnumerable<string> roles, string email)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

        var utcNow = DateTime.UtcNow;
        var issuer = _configuration["JWTKey:ValidIssuer"];
        var audience = _configuration["JWTKey:ValidAudience"];
        var expiryMinutes = int.Parse(_configuration["JWTKey:TokenExpiryTimeInMinutes"] ?? "30");
        var secret = _configuration["JWTKey:Secret"] ?? throw new InvalidOperationException("JWT secret is missing.");
        var baseKey = Encoding.UTF8.GetBytes(secret);

        var roleList = (roles ?? Enumerable.Empty<string>()).OrderBy(r => r).ToList();

        var derivedKey = DeriveKmacKey(userId, roleList, email, baseKey);

        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, userId),
        new(JwtRegisteredClaimNames.Email, email),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new(ClaimTypes.NameIdentifier, userId),
        new(ClaimTypes.Email, email)
    };

        claims.AddRange(roleList.Select(role => new Claim("role", role)));

        var credentials = new SigningCredentials(new SymmetricSecurityKey(derivedKey), SecurityAlgorithms.HmacSha512);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = utcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public static byte[] DeriveKmacKey(string userId, IEnumerable<string> roles, string email, byte[] secret)
    {
        var rolesString = string.Join(",", roles.OrderBy(r => r));
        var inputData = Encoding.UTF8.GetBytes($"{userId}|{rolesString}|{email}");

        var kmac = new Org.BouncyCastle.Crypto.Macs.KMac(256, secret);
        kmac.Init(new Org.BouncyCastle.Crypto.Parameters.KeyParameter(secret));
        kmac.BlockUpdate(inputData, 0, inputData.Length);
        var output = new byte[kmac.GetMacSize()];
        kmac.DoFinal(output, 0);
        return output;
    }

}