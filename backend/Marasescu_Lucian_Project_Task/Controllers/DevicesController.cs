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
        var created = await _deviceService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<DeviceResponseDto>> Update(int id, [FromBody] DeviceUpdateDto dto)
    {
        var updated = await _deviceService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _deviceService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
