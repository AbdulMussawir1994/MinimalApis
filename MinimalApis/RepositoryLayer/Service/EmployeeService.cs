using Microsoft.EntityFrameworkCore;
using MinimalApis.DataDbContext;
using MinimalApis.Entities.Model;
using MinimalApis.RepositoryLayer.Interface;

namespace MinimalApis.RepositoryLayer.Service;

public class EmployeeService(AppDbContext context) : IEmployeeService
{
    public async Task<IEnumerable<Employee>> GetAllAsync() =>
        await context.Employees.Include(e => e.Department).AsNoTracking().ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id) =>
        await context.Employees.Include(e => e.Department).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

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
