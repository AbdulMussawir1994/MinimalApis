using System.ComponentModel.DataAnnotations;

namespace MinimalApis.Entities.DTO;

public record struct LoginRequestDto
{
    [Required]
    public string userName { get; set; }
    [Required]
    public string password { get; set; }
}

public readonly record struct LoginResponseModelDto(string token);
