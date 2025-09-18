using Microsoft.EntityFrameworkCore;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;
using MinimalApis.RepositoryLayer.Interface;

namespace MinimalApis.RepositoryLayer.Service;

public class EmployeeService(AppDbContext context) : IEmployeeService
{
    public async Task<IReadOnlyList<GetEmployeeDto>> GetAllAsync()
    {
        var employees = await context.Employees
            .AsNoTracking()
            .Select(e => new GetEmployeeDto
            {
                id = e.Id,
                name = e.Name,
                departmentId = e.DepartmentId,
                department = e.Department != null ? e.Department.Name : string.Empty,
                email = e.Email
            })
            .ToListAsync();

        // Return a cached empty array if no employees are found
        return !employees.Any() ? Array.Empty<GetEmployeeDto>() : employees;
    }

    public async Task<GetEmployeeDto?> GetByIdAsync(int id)
    {
        var result = await context.Employees
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new GetEmployeeDto
            {
                id = e.Id,
                name = e.Name,
                departmentId = e.DepartmentId,
                department = e.Department != null ? e.Department.Name : string.Empty,
                email = e.Email
            })
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee?> UpdateAsync(int id, Employee updated)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is null) return null;

        employee.Name = updated.Name;
        employee.Email = updated.Email;
        employee.DepartmentId = updated.DepartmentId;

        await context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee is null) return false;

        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
        return true;
    }
}
