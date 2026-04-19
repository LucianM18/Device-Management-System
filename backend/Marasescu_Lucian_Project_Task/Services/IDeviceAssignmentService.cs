namespace Marasescu_Lucian_Project_Task.Services;

public interface IDeviceAssignmentService
{
    Task AssignDeviceAsync(int deviceId, int userId);
    Task UnassignDeviceAsync(int deviceId, int userId);
}
