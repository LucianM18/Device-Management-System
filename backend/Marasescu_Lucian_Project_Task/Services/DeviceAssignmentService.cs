using Marasescu_Lucian_Project_Task.Data;
using Marasescu_Lucian_Project_Task.Entities;
using Marasescu_Lucian_Project_Task.Repositories;

namespace Marasescu_Lucian_Project_Task.Services;

public class DeviceAssignmentService : IDeviceAssignmentService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly AppDbContext _context;

    public DeviceAssignmentService(IDeviceRepository deviceRepository, AppDbContext context)
    {
        _deviceRepository = deviceRepository;
        _context = context;
    }

    public async Task AssignDeviceAsync(int deviceId, int userId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device is null)
            throw new KeyNotFoundException("Device not found.");

        var existing = await _deviceRepository.GetActiveAssignmentAsync(deviceId);
        if (existing is not null)
            throw new InvalidOperationException("This device is already assigned to another user.");

        var assignment = new DeviceAssignment
        {
            DeviceId = deviceId,
            UserId = userId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.DeviceAssignments.Add(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task UnassignDeviceAsync(int deviceId, int userId)
    {
        var assignment = await _deviceRepository.GetActiveAssignmentForUserAsync(deviceId, userId);
        if (assignment is null)
            throw new KeyNotFoundException("No active assignment found for this device and user.");

        assignment.IsActive = false;
        assignment.ReleasedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
