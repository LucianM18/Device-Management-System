namespace Marasescu_Lucian_Project_Task.Dtos;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}
