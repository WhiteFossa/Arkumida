using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService
    (
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public async Task<string> GetConfigurationStringAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(nameof(key));
        }
        
        var value = _configuration[key];

        if (value == null)
        {
            throw new ArgumentException(nameof(key));
        }

        return value;
    }

    public async Task<int> GetConfigurationIntAsync(string key)
    {
        return int.Parse(await GetConfigurationStringAsync(key));
    }
}