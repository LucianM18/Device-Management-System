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
    public async Task UpdateDevice_ReturnsOkWithPartialUpdate()
    {
        var allResponse = await _client.GetAsync("/api/devices");
        var allDevices = await allResponse.Content.ReadFromJsonAsync<List<DeviceResponseDto>>();
        Assert.NotNull(allDevices);
        Assert.NotEmpty(allDevices);

        var targetId = allDevices[0].Id;
        var originalDevice = allDevices[0];

        var updateDto = new DeviceUpdateDto
        {
            Name = "Updated Name Only"
        };

        var patchResponse = await _client.PatchAsJsonAsync($"/api/devices/{targetId}", updateDto);

        Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);

        var updated = await patchResponse.Content.ReadFromJsonAsync<DeviceResponseDto>();
        Assert.NotNull(updated);
        Assert.Equal("Updated Name Only", updated.Name);

        Assert.Equal(originalDevice.Manufacturer, updated.Manufacturer);
        Assert.Equal(originalDevice.Processor, updated.Processor);
        Assert.Equal(originalDevice.RamAmount, updated.RamAmount);
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
            RamAmount = 4
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
