using System.ComponentModel.DataAnnotations;

namespace Marasescu_Lucian_Project_Task.Dtos;

public class DeviceCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string OperatingSystem { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string OsVersion { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Processor { get; set; } = null!;

    [Range(1, 128)]
    public int RamAmount { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}