using Marasescu_Lucian_Project_Task.Data;
using Marasescu_Lucian_Project_Task.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marasescu_Lucian_Project_Task.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real SQL Server DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add InMemory DB with a fixed name so all requests share the same instance
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestDb"));
        });

        builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Seed using the real app's service provider
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        SeedData(db);

        return host;
    }

    private static void SeedData(AppDbContext db)
    {
        if (db.Devices.Any()) return;

        db.Devices.AddRange(
            new Device
            {
                Name = "Seeded Laptop",
                Manufacturer = "Dell",
                Type = "Laptop",
                OperatingSystem = "Windows",
                OsVersion = "11",
                Processor = "Intel Core i7",
                RamAmount = 16,
                Description = "Seed device for tests"
            },
            new Device
            {
                Name = "Seeded Phone",
                Manufacturer = "Samsung",
                Type = "Phone",
                OperatingSystem = "Android",
                OsVersion = "14",
                Processor = "Snapdragon 8 Gen 2",
                RamAmount = 8
            }
        );

        db.SaveChanges();
    }
}
