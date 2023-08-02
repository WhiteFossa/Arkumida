namespace webapi.Services.Abstract;

/// <summary>
/// Service to read configuration from appsettings.json
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Get string from configuration (with check is it set)
    /// </summary>
    Task<string> GetConfigurationStringAsync(string key);

    /// <summary>
    /// Get int from configuration
    /// </summary>
    Task<int> GetConfigurationIntAsync(string key);
}