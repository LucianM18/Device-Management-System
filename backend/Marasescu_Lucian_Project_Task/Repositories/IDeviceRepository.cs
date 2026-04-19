using Marasescu_Lucian_Project_Task.Entities;

namespace Marasescu_Lucian_Project_Task.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<IEnumerable<Device>> GetAllWithCurrentUserAsync();
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<DeviceAssignment?> GetActiveAssignmentAsync(int deviceId);
    Task<DeviceAssignment?> GetActiveAssignmentForUserAsync(int deviceId, int userId);
}
