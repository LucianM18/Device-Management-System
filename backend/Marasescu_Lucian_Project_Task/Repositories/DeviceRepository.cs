using System.Linq.Expressions;
using System.Reflection;
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

    public async Task<DeviceAssignment?> GetActiveAssignmentAsync(int deviceId)
    {
        return await _context.DeviceAssignments
            .Include(da => da.User)
            .FirstOrDefaultAsync(da => da.DeviceId == deviceId && da.IsActive);
    }

    public async Task<DeviceAssignment?> GetActiveAssignmentForUserAsync(int deviceId, int userId)
    {
        return await _context.DeviceAssignments
            .FirstOrDefaultAsync(da => da.DeviceId == deviceId && da.UserId == userId && da.IsActive);
    }

    public async Task<(List<Device> Items, int TotalCount)> SearchPagedAsync(
        string[] tokens, int page, int pageSize)
    {
        if (tokens.Length == 0)
        {
            var baseQuery = _context.Devices
                .Include(d => d.DeviceAssignments.Where(da => da.IsActive))
                    .ThenInclude(da => da.User)
                .AsNoTracking();

            var totalCount = await baseQuery.CountAsync();
            var items = await baseQuery
                .OrderBy(d => d.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        var scoreExpr = BuildScoreExpression(tokens);
        var param = scoreExpr.Parameters[0];

        var selectExpr = Expression.Lambda<Func<Device, ScoredDeviceKey>>(
            Expression.MemberInit(
                Expression.New(typeof(ScoredDeviceKey)),
                Expression.Bind(
                    typeof(ScoredDeviceKey).GetProperty(nameof(ScoredDeviceKey.Id))!,
                    Expression.Property(param, nameof(Device.Id))),
                Expression.Bind(
                    typeof(ScoredDeviceKey).GetProperty(nameof(ScoredDeviceKey.Score))!,
                    scoreExpr.Body),
                Expression.Bind(
                    typeof(ScoredDeviceKey).GetProperty(nameof(ScoredDeviceKey.Name))!,
                    Expression.Property(param, nameof(Device.Name)))
            ),
            param);

        var scoredQuery = _context.Devices.AsNoTracking()
            .Select(selectExpr)
            .Where(x => x.Score > 0);

        var total = await scoredQuery.CountAsync();

        var pageKeys = await scoredQuery
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (pageKeys.Count == 0)
            return ([], total);

        var ids = pageKeys.Select(x => x.Id).ToList();
        var devices = await _context.Devices
            .Include(d => d.DeviceAssignments.Where(da => da.IsActive))
                .ThenInclude(da => da.User)
            .AsNoTracking()
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();

        var ordered = pageKeys
            .Select(k => devices.First(d => d.Id == k.Id))
            .ToList();

        return (ordered, total);
    }

    private static readonly MethodInfo ToLowerMethod =
        typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;

    private static readonly MethodInfo StringContainsMethod =
        typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

    private static readonly MethodInfo IntToStringMethod =
        typeof(int).GetMethod(nameof(int.ToString), Type.EmptyTypes)!;

    private static Expression<Func<Device, double>> BuildScoreExpression(string[] tokens)
    {
        var param = Expression.Parameter(typeof(Device), "d");
        Expression score = Expression.Constant(0.0);

        foreach (var token in tokens)
        {
            var tokenConst = Expression.Constant(token);

            Expression FieldScore(string propertyName, double weight)
            {
                var prop = Expression.Property(param, propertyName);
                var lower = Expression.Call(prop, ToLowerMethod);
                var contains = Expression.Call(lower, StringContainsMethod, tokenConst);
                return Expression.Condition(contains,
                    Expression.Constant(weight),
                    Expression.Constant(0.0));
            }

            score = Expression.Add(score, FieldScore(nameof(Device.Name), 4.0));
            score = Expression.Add(score, FieldScore(nameof(Device.Manufacturer), 3.0));
            score = Expression.Add(score, FieldScore(nameof(Device.Processor), 2.0));

            var ramProp = Expression.Property(param, nameof(Device.RamAmount));
            var ramStr = Expression.Call(ramProp, IntToStringMethod);
            var ramContains = Expression.Call(ramStr, StringContainsMethod, tokenConst);
            score = Expression.Add(score,
                Expression.Condition(ramContains,
                    Expression.Constant(1.0),
                    Expression.Constant(0.0)));
        }

        return Expression.Lambda<Func<Device, double>>(score, param);
    }

    private class ScoredDeviceKey
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public string Name { get; set; } = "";
    }
}


