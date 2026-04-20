using System.Text.RegularExpressions;
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
        return devices.Select(d => MapToResponseDto(d));
    }



    public async Task<DeviceResponseDto?> GetByIdAsync(int id)
    {
        var device = await _repository.GetByIdAsync(id);
        if (device is null) return null;

        var assignment = await _repository.GetActiveAssignmentAsync(id);
        return MapToResponseDto(device, assignment);
    }

    public async Task<IEnumerable<DeviceListItemDto>> GetAllWithCurrentUserAsync()
    {
        var devices = await _repository.GetAllWithCurrentUserAsync();

        return devices.Select(device => new DeviceListItemDto
        {
            Id = device.Id,
            Name = device.Name,
            Manufacturer = device.Manufacturer,
            Type = device.Type,
            OperatingSystem = device.OperatingSystem,
            OsVersion = device.OsVersion,
            Processor = device.Processor,
            RamAmount = device.RamAmount,
            Description = device.Description,
            CurrentUserName = device.DeviceAssignments
                .FirstOrDefault(da => da.IsActive)?.User?.Name,
            CurrentUserRole = device.DeviceAssignments
                .FirstOrDefault(da => da.IsActive)?.User?.Role
        });
    }

    public async Task<DeviceResponseDto> CreateAsync(DeviceCreateDto dto)
    {
        var device = new Device
        {
            Name = dto.Name.Trim(),
            Manufacturer = dto.Manufacturer.Trim(),
            Type = dto.Type.Trim(),
            OperatingSystem = dto.OperatingSystem.Trim(),
            OsVersion = dto.OsVersion.Trim(),
            Processor = dto.Processor.Trim(),
            RamAmount = dto.RamAmount,
            Description = dto.Description.Trim()
        };

        var created = await _repository.CreateAsync(device);
        return MapToResponseDto(created);
    }

    public async Task<DeviceResponseDto?> UpdateAsync(int id, DeviceUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        existing.Name = dto.Name.Trim();
        existing.Manufacturer = dto.Manufacturer.Trim();
        existing.Type = dto.Type.Trim();
        existing.OperatingSystem = dto.OperatingSystem.Trim();
        existing.OsVersion = dto.OsVersion.Trim();
        existing.Processor = dto.Processor.Trim();
        existing.RamAmount = dto.RamAmount;
        existing.Description = dto.Description.Trim();

        await _repository.UpdateAsync(existing);
        return MapToResponseDto(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<PaginatedResult<DeviceListItemDto>> SearchAsync(string? query, int page, int pageSize)
    {
        var tokens = NormalizeQuery(query);
        var (devices, totalCount) = await _repository.SearchPagedAsync(tokens, page, pageSize);

        return new PaginatedResult<DeviceListItemDto>
        {
            Items = devices.Select(MapToListItemDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    private static string[] NormalizeQuery(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var lower = query.Trim().ToLowerInvariant();
        var stripped = Regex.Replace(lower, @"[^\w\s]", " ");
        return stripped.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    private static DeviceListItemDto MapToListItemDto(Device device) => new()
    {
        Id = device.Id,
        Name = device.Name,
        Manufacturer = device.Manufacturer,
        Type = device.Type,
        OperatingSystem = device.OperatingSystem,
        OsVersion = device.OsVersion,
        Processor = device.Processor,
        RamAmount = device.RamAmount,
        Description = device.Description,
        CurrentUserName = device.DeviceAssignments.FirstOrDefault(da => da.IsActive)?.User?.Name,
        CurrentUserRole = device.DeviceAssignments.FirstOrDefault(da => da.IsActive)?.User?.Role
    };

    private static DeviceResponseDto MapToResponseDto(Device device, Marasescu_Lucian_Project_Task.Entities.DeviceAssignment? assignment = null) => new()
    {
        Id = device.Id,
        Name = device.Name,
        Manufacturer = device.Manufacturer,
        Type = device.Type,
        OperatingSystem = device.OperatingSystem,
        OsVersion = device.OsVersion,
        Processor = device.Processor,
        RamAmount = device.RamAmount,
        Description = device.Description,
        CurrentUserId = assignment?.UserId,
        CurrentUserName = assignment?.User?.Name
    };
}
