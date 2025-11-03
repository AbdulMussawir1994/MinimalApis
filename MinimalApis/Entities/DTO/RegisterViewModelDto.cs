using System.ComponentModel.DataAnnotations;

namespace MinimalApis.Entities.DTO;


public readonly record struct RegisterViewModelDto
{
    [Required]
    public string Username { get; init; }
    [Required]
    public string Email { get; init; }
    [Required]
    public string Password { get; init; }
    [Required]
    public string ConfirmPassword { get; init; }
}
