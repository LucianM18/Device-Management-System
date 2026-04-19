using System.Security.Claims;
using Marasescu_Lucian_Project_Task.Dtos;
using Marasescu_Lucian_Project_Task.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marasescu_Lucian_Project_Task.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserInfoDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var user = await _authService.RegisterAsync(dto);
            return CreatedAtAction(nameof(Me), user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserInfoDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await _authService.LoginAsync(dto);

        if (user is null)
            return Unauthorized(new { message = "Invalid email or password." });

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(user);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<UserInfoDto> Me()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idClaim is null)
            return Unauthorized();

        return Ok(new UserInfoDto
        {
            Id = int.Parse(idClaim),
            Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Name = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
        });
    }
}
