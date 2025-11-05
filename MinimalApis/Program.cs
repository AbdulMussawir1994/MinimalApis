using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.Helpers;
using MinimalApis.RepositoryLayer.Interface;
using MinimalApis.RepositoryLayer.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds)
    )
);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowCors", x =>
    {
        x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(1));
    });
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(configuration["JWTKey:Secret"]);

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        // IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.Secret)),
        ValidateAudience = true,
        ValidIssuer = configuration["JWTKey:ValidIssuer"],
        ValidAudience = configuration["JWTKey:ValidAudience"],
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
    };
});
builder.Services.AddAuthorization();

//builder.Services.AddAuthorization(options =>
//{
//    // Global rule: everything requires JWT unless explicitly marked AllowAnonymous
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    //   options.SwaggerDoc("v2", new OpenApiInfo { Title = "Library APIS", Version = "v2" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter your JWT token with **Bearer** prefix.\nExample: Bearer eyJhbGciOi...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = "swagger"; // opens at https://localhost:7170/swagger/index.html
});

app.UseHttpsRedirection();

app.UseCors("AllowCors");

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", () => Results.Ok("✅ Minimal API with Clean Architecture is running!"));

// Employee Endpoints
app.MapGet("/employees", async (IEmployeeService service) =>
    Results.Ok(await service.GetAllAsync()))
    .RequireAuthorization();
//.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" }); // ✅ Only Admin role

// Outlet Endpoints
app.MapGet("/outlets", async (IUserService service) =>
    Results.Ok(await service.GetOutletsAsync()))
    .RequireAuthorization();

//Get Roles
app.MapPost("/GetRolesById", async (GetRolesById model, IUserService service) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var result = await service.GetRolesByIdAsync(model);
    return result.Status ? Results.Ok(result) : Results.BadRequest(result);

}).RequireAuthorization();

//Get Employee By Id
app.MapGet("/employees/{id:int}", async (int id, IEmployeeService service) =>
{
    var employee = await service.GetByIdAsync(id);
    return employee.Value.name is not null ? Results.Ok(employee) : Results.NotFound();
});

// Add Employee
app.MapPost("/employees", async (EmployeeDto dto, IEmployeeService service) =>
{
    var employee = new Employee
    {
        Name = dto.Name,
        Email = dto.Email,
        DepartmentId = dto.DepartmentId
    };

    var created = await service.AddAsync(employee);
    return Results.Created($"/employees/{created.Id}", created);
});

//Add Outlet
app.MapPost("/outlets", async (AddOutletDto model, IUserService service) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var result = await service.AddOutletAsync(model);

    if (!result.Status)
        return Results.BadRequest(result);

    return Results.Created($"/outlets/{result.Data}", result);
})
.WithName("AddOutlet")
.Produces<GenericResponse<OutletDto>>(StatusCodes.Status201Created)
.Produces<GenericResponse<OutletDto>>(StatusCodes.Status400BadRequest)
.WithOpenApi()
.RequireAuthorization();

// User Login
app.MapPost("/login", async (LoginRequestDto model, IUserService service, CancellationToken ct) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var response = await service.LoginUserAsync(model, ct);
    return response.Status ? Results.Ok(response) : Results.BadRequest(response);
})
    .AllowAnonymous()
.WithName("Login")
.WithOpenApi()
.Produces<GenericResponse<LoginResponseModelDto>>(StatusCodes.Status200OK)
.Produces<GenericResponse<LoginResponseModelDto>>(StatusCodes.Status400BadRequest);

// User Register
app.MapPost("/register", async (RegisterViewModelDto model, IUserService service, CancellationToken ct) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var response = await service.RegisterUserAsync(model, ct);
    return response.Status ? Results.Ok(response) : Results.BadRequest(response);
})
    .AllowAnonymous()
.WithName("Register")
.WithOpenApi()
.Produces<GenericResponse<LoginResponseModelDto>>(StatusCodes.Status200OK)
.Produces<GenericResponse<LoginResponseModelDto>>(StatusCodes.Status400BadRequest);

// Employee Update
app.MapPut("/employees/{id:int}", async (int id, EmployeeDto dto, IEmployeeService service) =>
{
    var updatedEmployee = new Employee
    {
        Name = dto.Name,
        Email = dto.Email,
        DepartmentId = dto.DepartmentId
    };

    var updated = await service.UpdateAsync(id, updatedEmployee);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

// Employee Delete
app.MapDelete("/employees/{id:int}", async (int id, IEmployeeService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});


app.Run();