namespace Marasescu_Lucian_Project_Task.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Location { get; set; } = null!;
        public ICollection<DeviceAssignment> DeviceAssignments { get; set; } = new List<DeviceAssignment>();

    }
}
