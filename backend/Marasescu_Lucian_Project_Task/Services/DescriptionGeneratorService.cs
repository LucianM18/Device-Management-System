using System.Text.Json;

namespace Marasescu_Lucian_Project_Task.Services;

public class DescriptionGeneratorService : IDescriptionGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public DescriptionGeneratorService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini API key is not configured.");
    }

    public async Task<string> GenerateDescriptionAsync(
        string name,
        string manufacturer,
        string type,
        string operatingSystem,
        string processor,
        int ramAmount)
    {
        var prompt =
            $"Given the following device details, generate a single concise sentence " +
            $"professional description suitable for an IT asset catalog. " +
            $"Only output the description sentence, nothing else. " +
            $"Here is an example you can follow:" +
            $"Input: Name - iPhone 17 Pro, Manufacturer - Apple, OS - iOS, Type - phone, RAM - 12GB, Processor - A19 Pro" +
            $"Output: 'A high-performance Apple smartphone running iOS, suitable for daily business use.'" +
            $"Name: {name}, Manufacturer: {manufacturer}, Type: {type}, " +
            $"Operating System: {operatingSystem}, Processor: {processor}, RAM: {ramAmount}GB";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent?key={_apiKey}";

        var response = await _httpClient.PostAsJsonAsync(url, requestBody);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        var text = json
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text?.Trim() ?? string.Empty;
    }
}
