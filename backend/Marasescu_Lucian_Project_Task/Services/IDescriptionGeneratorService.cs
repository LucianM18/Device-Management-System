namespace Marasescu_Lucian_Project_Task.Services;

public interface IDescriptionGeneratorService
{
    Task<string> GenerateDescriptionAsync(
        string name,
        string manufacturer,
        string type,
        string operatingSystem,
        string processor,
        int ramAmount);
}