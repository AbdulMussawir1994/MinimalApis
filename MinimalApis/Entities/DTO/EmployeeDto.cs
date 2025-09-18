namespace MinimalApis.Entities.DTO;

public class EmployeeDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}

public readonly record struct GetEmployeeDto(int id, string name, string email, string department, int departmentId);