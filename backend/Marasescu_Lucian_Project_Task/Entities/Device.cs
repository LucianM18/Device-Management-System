namespace Marasescu_Lucian_Project_Task.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string OperatingSystem { get; set; } = null!;
        public string OsVersion { get; set; } = null!;
        public string Processor { get; set; } = null!;
        public int RamAmount { get; set; }
        public string? Description { get; set; }

        public ICollection<DeviceAssignment> DeviceAssignments { get; set; } = new List<DeviceAssignment>();

    }
}
