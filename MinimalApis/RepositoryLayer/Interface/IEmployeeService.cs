using MinimalApis.Entities.DTO;
using MinimalApis.Entities.Model;

namespace MinimalApis.RepositoryLayer.Interface;

public interface IEmployeeService
{
    Task<IReadOnlyList<GetEmployeeDto>> GetAllAsync();
    Task<GetEmployeeDto?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee?> UpdateAsync(int id, Employee employee);
    Task<bool> DeleteAsync(int id);
}