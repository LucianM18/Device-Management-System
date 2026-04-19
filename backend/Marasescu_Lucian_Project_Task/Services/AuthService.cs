using Marasescu_Lucian_Project_Task.Data;
using Marasescu_Lucian_Project_Task.Dtos;
using Marasescu_Lucian_Project_Task.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marasescu_Lucian_Project_Task.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserInfoDto> RegisterAsync(RegisterDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToLower();

        var exists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == normalizedEmail);

        if (exists)
            throw new InvalidOperationException("A user with this email already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            Role = "User",
            Location = string.Empty
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserInfoDto?> LoginAsync(LoginDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToLower();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return MapToDto(user);
    }

    public async Task<UserInfoDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user is null ? null : MapToDto(user);
    }

    private static UserInfoDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Name = user.Name,
        Role = user.Role
    };
}
