using Marasescu_Lucian_Project_Task.Dtos;

namespace Marasescu_Lucian_Project_Task.Services;

public interface IAuthService
{
    Task<UserInfoDto> RegisterAsync(RegisterDto dto);
    Task<UserInfoDto?> LoginAsync(LoginDto dto);
    Task<UserInfoDto?> GetByIdAsync(int id);
}
