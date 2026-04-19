using System.ComponentModel.DataAnnotations;

namespace Marasescu_Lucian_Project_Task.Dtos;

public class GenerateDescriptionRequestDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Manufacturer { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;

    [Required]
    public string OperatingSystem { get; set; } = null!;

    [Required]
    public string Processor { get; set; } = null!;

    [Range(1, 128)]
    public int RamAmount { get; set; }
}