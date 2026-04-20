using Marasescu_Lucian_Project_Task.Entities;

namespace Marasescu_Lucian_Project_Task.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetAllWithCurrentUserAsync();
    Task<DeviceAssignment?> GetActiveAssignmentAsync(int deviceId);
    Task<DeviceAssignment?> GetActiveAssignmentForUserAsync(int deviceId, int userId);
    Task<(List<Device> Items, int TotalCount)> SearchPagedAsync(string[] tokens, int page, int pageSize);
}
