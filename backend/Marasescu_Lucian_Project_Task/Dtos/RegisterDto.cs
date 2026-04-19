using System.ComponentModel.DataAnnotations;

namespace Marasescu_Lucian_Project_Task.Dtos;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}
