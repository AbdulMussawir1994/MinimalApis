using Microsoft.EntityFrameworkCore;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.RepositoryLayer.Interface;
using MinimalApis.RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext (High performance: UsePooling)

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    }));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Register services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok("✅ Minimal API with Clean Architecture is running!"));

// Employee Endpoints
app.MapGet("/employees", async (IEmployeeService service) =>
    Results.Ok(await service.GetAllAsync()));

app.MapGet("/employees/{id:int}", async (int id, IEmployeeService service) =>
{
    var employee = await service.GetByIdAsync(id);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

// POST - Create Employee
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

// PUT - Update Employee
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

app.MapDelete("/employees/{id:int}", async (int id, IEmployeeService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();