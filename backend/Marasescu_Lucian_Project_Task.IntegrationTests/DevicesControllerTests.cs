using System.Net;
using System.Net.Http.Json;
using Marasescu_Lucian_Project_Task.Dtos;

namespace Marasescu_Lucian_Project_Task.IntegrationTests;

public class DevicesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DevicesControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllDevices_ReturnsOkWithSeededData()
    {
        var response = await _client.GetAsync("/api/devices");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var devices = await response.Content.ReadFromJsonAsync<List<DeviceResponseDto>>();
        Assert.NotNull(devices);
        Assert.True(devices.Count >= 2);
    }

    [Fact]
    public async Task CreateDevice_ReturnsCreatedAndPersists()
    {
        var newDevice = new DeviceCreateDto
        {
            Name = "Integration Test Device",
            Manufacturer = "Apple",
            Type = "Laptop",
            OperatingSystem = "macOS",
            OsVersion = "14",
            Processor = "Apple M3",
            RamAmount = 32,
            Description = "Created in integration test"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/devices", newDevice);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<DeviceResponseDto>();
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(newDevice.Name, created.Name);
        Assert.Equal(newDevice.Manufacturer, created.Manufacturer);

        var getResponse = await _client.GetAsync($"/api/devices/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateDevice_WithInvalidData_ReturnsBadRequest()
    {
        var invalidDevice = new
        {
            RamAmount = 0
        };

        var response = await _client.PostAsJsonAsync("/api/devices", invalidDevice);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateDevice_WithPut_ReturnsOkWithFullUpdate()
    {
        var allResponse = await _client.GetAsync("/api/devices");
        var allDevices = await allResponse.Content.ReadFromJsonAsync<List<DeviceResponseDto>>();
        Assert.NotNull(allDevices);
        Assert.NotEmpty(allDevices);

        var targetId = allDevices[0].Id;

        var updateDto = new DeviceUpdateDto
        {
            Name = "Updated Full Device",
            Manufacturer = "Lenovo",
            Type = "Laptop",
            OperatingSystem = "Windows",
            OsVersion = "11 Pro",
            Processor = "Intel Core i9",
            RamAmount = 32,
            Description = "Updated via PUT"
        };

        var putResponse = await _client.PutAsJsonAsync($"/api/devices/{targetId}", updateDto);

        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

        var updated = await putResponse.Content.ReadFromJsonAsync<DeviceResponseDto>();
        Assert.NotNull(updated);
        Assert.Equal(updateDto.Name, updated.Name);
        Assert.Equal(updateDto.Manufacturer, updated.Manufacturer);
        Assert.Equal(updateDto.Type, updated.Type);
        Assert.Equal(updateDto.OperatingSystem, updated.OperatingSystem);
        Assert.Equal(updateDto.OsVersion, updated.OsVersion);
        Assert.Equal(updateDto.Processor, updated.Processor);
        Assert.Equal(updateDto.RamAmount, updated.RamAmount);
        Assert.Equal(updateDto.Description, updated.Description);
    }

    [Fact]
    public async Task UpdateDevice_WithInvalidData_ReturnsBadRequest()
    {
        var allResponse = await _client.GetAsync("/api/devices");
        var allDevices = await allResponse.Content.ReadFromJsonAsync<List<DeviceResponseDto>>();
        Assert.NotNull(allDevices);
        Assert.NotEmpty(allDevices);

        var targetId = allDevices[0].Id;
        var invalidUpdate = new
        {
            Name = "   ",
            Manufacturer = "Lenovo",
            Type = "Laptop",
            OperatingSystem = "Windows",
            OsVersion = "11",
            Processor = "Intel",
            RamAmount = 16,
            Description = "Valid description"
        };

        var response = await _client.PutAsJsonAsync($"/api/devices/{targetId}", invalidUpdate);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteDevice_ReturnsNoContentAndRemoves()
    {
        var newDevice = new DeviceCreateDto
        {
            Name = "Device To Delete",
            Manufacturer = "Test Corp",
            Type = "Tablet",
            OperatingSystem = "Android",
            OsVersion = "13",
            Processor = "MediaTek",
            RamAmount = 4,
            Description = "Delete test device"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/devices", newDevice);
        var created = await createResponse.Content.ReadFromJsonAsync<DeviceResponseDto>();
        Assert.NotNull(created);

        var deleteResponse = await _client.DeleteAsync($"/api/devices/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/devices/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
