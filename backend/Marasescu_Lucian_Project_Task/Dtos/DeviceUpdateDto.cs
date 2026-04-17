using System.ComponentModel.DataAnnotations;

namespace Marasescu_Lucian_Project_Task.Dtos;

public class DeviceUpdateDto
{

    [MaxLength(100)]
    public string? Name { get; set; }


    [MaxLength(100)]
    public string? Manufacturer { get; set; }


    [MaxLength(20)]
    public string? Type { get; set; }


    [MaxLength(50)]
    public string? OperatingSystem { get; set; }


    [MaxLength(50)]
    public string? OsVersion { get; set; }


    [MaxLength(100)]
    public string? Processor { get; set; }


    [Range(1, 128)]
    public int? RamAmount { get; set; }


    [MaxLength(500)]
    public string? Description { get; set; }
}