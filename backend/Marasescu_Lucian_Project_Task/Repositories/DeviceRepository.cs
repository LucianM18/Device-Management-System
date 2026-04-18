using Marasescu_Lucian_Project_Task.Data;
using Marasescu_Lucian_Project_Task.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marasescu_Lucian_Project_Task.Repositories;

public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    public DeviceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Device>> GetAllWithCurrentUserAsync()
    {
        return await _context.Devices
            .Include(d => d.DeviceAssignments.Where(da => da.IsActive))
                .ThenInclude(da => da.User)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.Devices.AnyAsync(d =>
            (!excludeId.HasValue || d.Id != excludeId.Value) &&
            d.Name.ToLower() == normalizedName);
    }
}


