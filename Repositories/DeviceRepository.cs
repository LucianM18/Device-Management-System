using Marasescu_Lucian_Project_Task.Data;
using Marasescu_Lucian_Project_Task.Entities;

namespace Marasescu_Lucian_Project_Task.Repositories;

public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    public DeviceRepository(AppDbContext context) : base(context)
    {
    }

}
