using Marasescu_Lucian_Project_Task.Dtos;
using Marasescu_Lucian_Project_Task.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marasescu_Lucian_Project_Task.Controllers;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
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
