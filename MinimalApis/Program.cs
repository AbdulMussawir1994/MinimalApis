using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.Helpers;
using MinimalApis.RepositoryLayer.Interface;
using MinimalApis.RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// -----------------------------------------------------
// 1️⃣ Configure JSON & Database
// -----------------------------------------------------
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

// -----------------------------------------------------
// 2️⃣ Identity, Authentication & Authorization
// -----------------------------------------------------
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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// -----------------------------------------------------
// 3️⃣ Dependency Injection (Services)
// -----------------------------------------------------
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// -----------------------------------------------------
// 4️⃣ Swagger (API Documentation)
// -----------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------
// 5️⃣ Build the App
// -----------------------------------------------------
var app = builder.Build();

// -----------------------------------------------------
// 6️⃣ Middleware Pipeline
// -----------------------------------------------------
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}

// Serve static files if needed
app.UseStaticFiles();

// ✅ Swagger middleware (UI at /swagger)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = "swagger"; // opens at https://localhost:7170/swagger/index.html
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// -----------------------------------------------------
// 7️⃣ Minimal API Endpoints
// -----------------------------------------------------
app.MapGet("/", () => Results.Ok("✅ Minimal API with Clean Architecture is running!"));

// Employee Endpoints
app.MapGet("/employees", async (IEmployeeService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/employees/{id:int}", async (int id, IEmployeeService service) =>
{
    var employee = await service.GetByIdAsync(id);
    return employee.Value.name is not null ? Results.Ok(employee) : Results.NotFound();
});

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

// User Login
app.MapPost("/login", async (LoginRequestDto model, IUserService service, CancellationToken ct) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var response = await service.LoginUserAsync(model, ct);
    return response.Status ? Results.Ok(response) : Results.BadRequest(response);
})
.WithName("Login")
.WithOpenApi()
.Produces<GenericsResponse<LoginResponseModelDto>>(StatusCodes.Status200OK)
.Produces<GenericsResponse<LoginResponseModelDto>>(StatusCodes.Status400BadRequest);

// User Register
app.MapPost("/register", async (RegisterViewModelDto model, IUserService service, CancellationToken ct) =>
{
    var validation = model.ValidateModel();
    if (validation is IStatusCodeHttpResult { StatusCode: 400 })
        return validation;

    var response = await service.RegisterUserAsync(model, ct);
    return response.Status ? Results.Ok(response) : Results.BadRequest(response);
})
.WithName("Register")
.WithOpenApi()
.Produces<GenericsResponse<LoginResponseModelDto>>(StatusCodes.Status200OK)
.Produces<GenericsResponse<LoginResponseModelDto>>(StatusCodes.Status400BadRequest);

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

// -----------------------------------------------------
// 8️⃣ Run the App
// -----------------------------------------------------
app.Run();