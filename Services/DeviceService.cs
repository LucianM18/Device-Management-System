using Marasescu_Lucian_Project_Task.Dtos;
using Marasescu_Lucian_Project_Task.Entities;
using Marasescu_Lucian_Project_Task.Repositories;

namespace Marasescu_Lucian_Project_Task.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;

    public DeviceService(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeviceResponseDto>> GetAllAsync()
    {
        var devices = await _repository.GetAllAsync();
        return devices.Select(MapToResponseDto);
    }

    public async Task<DeviceResponseDto?> GetByIdAsync(int id)
    {
        var device = await _repository.GetByIdAsync(id);
        return device is null ? null : MapToResponseDto(device);
    }

    public async Task<DeviceResponseDto> CreateAsync(DeviceCreateDto dto)
    {
        var device = new Device
        {
            Name = dto.Name,
            Manufacturer = dto.Manufacturer,
            Type = dto.Type,
            OperatingSystem = dto.OperatingSystem,
            OsVersion = dto.OsVersion,
            Processor = dto.Processor,
            RamAmount = dto.RamAmount,
            Description = dto.Description
        };

        var created = await _repository.CreateAsync(device);
        return MapToResponseDto(created);
    }

    public async Task<DeviceResponseDto?> UpdateAsync(int id, DeviceUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        if (dto.Name is not null)            existing.Name = dto.Name;
        if (dto.Manufacturer is not null)    existing.Manufacturer = dto.Manufacturer;
        if (dto.Type is not null)            existing.Type = dto.Type;
        if (dto.OperatingSystem is not null) existing.OperatingSystem = dto.OperatingSystem;
        if (dto.OsVersion is not null)       existing.OsVersion = dto.OsVersion;
        if (dto.Processor is not null)       existing.Processor = dto.Processor;
        if (dto.RamAmount.HasValue)          existing.RamAmount = dto.RamAmount.Value;
        if (dto.Description is not null)     existing.Description = dto.Description;

        await _repository.UpdateAsync(existing);
        return MapToResponseDto(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static DeviceResponseDto MapToResponseDto(Device device) => new()
    {
        Id = device.Id,
        Name = device.Name,
        Manufacturer = device.Manufacturer,
        Type = device.Type,
        OperatingSystem = device.OperatingSystem,
        OsVersion = device.OsVersion,
        Processor = device.Processor,
        RamAmount = device.RamAmount,
        Description = device.Description
    };
}
