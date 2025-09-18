using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApis.Entities.Model;

public class Department
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}