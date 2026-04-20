using Marasescu_Lucian_Project_Task.Dtos;

namespace Marasescu_Lucian_Project_Task.Services;

public interface IDeviceService
{
    Task<IEnumerable<DeviceResponseDto>> GetAllAsync();
    Task<IEnumerable<DeviceListItemDto>> GetAllWithCurrentUserAsync();
    Task<DeviceResponseDto?> GetByIdAsync(int id);
    Task<DeviceResponseDto> CreateAsync(DeviceCreateDto dto);
    Task<DeviceResponseDto?> UpdateAsync(int id, DeviceUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PaginatedResult<DeviceListItemDto>> SearchAsync(string? query, int page, int pageSize);
}
