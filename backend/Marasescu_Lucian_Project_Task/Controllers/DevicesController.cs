using System.Security.Claims;
using Marasescu_Lucian_Project_Task.Dtos;
using Marasescu_Lucian_Project_Task.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marasescu_Lucian_Project_Task.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IDeviceAssignmentService _assignmentService;
    private readonly IDescriptionGeneratorService _descriptionGeneratorService;

    public DevicesController(
        IDeviceService deviceService,
        IDeviceAssignmentService assignmentService,
        IDescriptionGeneratorService descriptionGeneratorService)
    {
        _deviceService = deviceService;
        _assignmentService = assignmentService;
        _descriptionGeneratorService = descriptionGeneratorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceResponseDto>>> GetAll()
    {
        var devices = await _deviceService.GetAllAsync();
        return Ok(devices);
    }

    [HttpGet("with-current-user")]
    public async Task<ActionResult<IEnumerable<DeviceListItemDto>>> GetAllWithCurrentUser()
    {
        var devices = await _deviceService.GetAllWithCurrentUserAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceResponseDto>> GetById(int id)
    {
        var device = await _deviceService.GetByIdAsync(id);
        if (device is null)
            return NotFound();

        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<DeviceResponseDto>> Create([FromBody] DeviceCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var payloadError = ValidateCreatePayload(dto);
        if (payloadError is not null)
            return BadRequest(new { message = payloadError });

        try
        {
            var created = await _deviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DeviceResponseDto>> Update(int id, [FromBody] DeviceUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var payloadError = ValidateUpdatePayload(dto);
        if (payloadError is not null)
            return BadRequest(new { message = payloadError });

        try
        {
            var updated = await _deviceService.UpdateAsync(id, dto);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _deviceService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("generate-description")]
    public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequestDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var description = await _descriptionGeneratorService.GenerateDescriptionAsync(
                dto.Name, dto.Manufacturer, dto.Type, dto.OperatingSystem, dto.Processor, dto.RamAmount);
            return Ok(new { description });
        }
        catch
        {
            return StatusCode(502, new { message = "Failed to generate description. Please try again." });
        }
    }

    [HttpPost("{id}/assign")]
    public async Task<IActionResult> Assign(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);

        try
        {
            await _assignmentService.AssignDeviceAsync(id, userId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/unassign")]
    public async Task<IActionResult> Unassign(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);

        try
        {
            await _assignmentService.UnassignDeviceAsync(id, userId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private static string? ValidateCreatePayload(DeviceCreateDto dto)
    {
        return ValidatePayload(
            dto.Name,
            dto.Manufacturer,
            dto.Type,
            dto.OperatingSystem,
            dto.OsVersion,
            dto.Processor,
            dto.RamAmount,
            dto.Description);
    }

    private static string? ValidateUpdatePayload(DeviceUpdateDto dto)
    {
        return ValidatePayload(
            dto.Name,
            dto.Manufacturer,
            dto.Type,
            dto.OperatingSystem,
            dto.OsVersion,
            dto.Processor,
            dto.RamAmount,
            dto.Description);
    }

    private static string? ValidatePayload(
        string name,
        string manufacturer,
        string type,
        string operatingSystem,
        string osVersion,
        string processor,
        int ramAmount,
        string description)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Name is required.";
        if (string.IsNullOrWhiteSpace(manufacturer)) return "Manufacturer is required.";
        if (string.IsNullOrWhiteSpace(type)) return "Type is required.";
        if (string.IsNullOrWhiteSpace(operatingSystem)) return "OperatingSystem is required.";
        if (string.IsNullOrWhiteSpace(osVersion)) return "OsVersion is required.";
        if (string.IsNullOrWhiteSpace(processor)) return "Processor is required.";
        if (string.IsNullOrWhiteSpace(description)) return "Description is required.";
        if (ramAmount is < 1 or > 128) return "RamAmount must be between 1 and 128.";

        return null;
    }
}
