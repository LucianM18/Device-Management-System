
using Marasescu_Lucian_Project_Task.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marasescu_Lucian_Project_Task.Data;


    public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<User> Users => Set<User>();
    public DbSet<DeviceAssignment> DeviceAssignments => Set<DeviceAssignment>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Devices");
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                     .HasMaxLength(100)
                     .IsRequired();

            entity.Property(d => d.Manufacturer)
                     .HasMaxLength(100)
                     .IsRequired();

            entity.Property(d => d.Type)
                     .HasMaxLength(20)
                     .IsRequired();

            entity.Property(d => d.OperatingSystem)
                     .HasMaxLength(50)
                     .IsRequired();

            entity.Property(d => d.OsVersion)
                     .HasMaxLength(50)
                     .IsRequired();

            entity.Property(d => d.Processor)
                     .HasMaxLength(100)
                     .IsRequired();

            entity.Property(d => d.Description)
                    .HasMaxLength(500);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Role)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Location)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        modelBuilder.Entity<DeviceAssignment>(entity =>
        {
            entity.ToTable("DeviceAssignments");
            entity.HasKey(da => da.Id);

            entity.Property(da => da.AssignedAt)
                .IsRequired();

            entity.Property(da => da.IsActive)
                .IsRequired();

            entity.HasOne(da => da.Device)
                .WithMany(d => d.DeviceAssignments)
                .HasForeignKey(da => da.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(da => da.User)
                .WithMany(u => u.DeviceAssignments)
                .HasForeignKey(da => da.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
