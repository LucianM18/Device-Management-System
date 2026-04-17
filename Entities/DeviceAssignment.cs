namespace Marasescu_Lucian_Project_Task.Entities;

public class DeviceAssignment
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public int UserId { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool IsActive { get; set; }

    public Device Device { get; set; } = null!;
    public User User { get; set; } = null!;
}