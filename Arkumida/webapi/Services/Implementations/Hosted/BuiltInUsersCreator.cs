using webapi.Constants;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations.Hosted;

/// <summary>
/// Service for creating built-in users
/// </summary>
public class BuiltInUsersCreator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public BuiltInUsersCreator
    (
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            // DI
            var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
            var accountsService = scope.ServiceProvider.GetRequiredService<IAccountsService>();
            
            // Creating user for importer
            var importerUserLogin = await configurationService.GetConfigurationStringAsync(GlobalConstants.ImporterUserLoginSettingName);
            var importerUserEmail = await configurationService.GetConfigurationStringAsync(GlobalConstants.ImporterUserEmailSettingName);
            var importerUserPassword = await configurationService.GetConfigurationStringAsync(GlobalConstants.ImporterUserPasswordSettingName);
            await CreateUserIfNotExistAsync(accountsService, importerUserLogin, importerUserEmail, importerUserPassword);
        }
    }

    private async Task CreateUserIfNotExistAsync(IAccountsService accountsService, string login, string email, string password)
    {
        if (await accountsService.IsUserExistByLoginAsync(login))
        {
            // Already exist
            return;
        }

        var registrationResult = await accountsService.RegisterUserAsync(new RegistrationDataDto() { Login = login, Email = email, Password = password });
        if (registrationResult.Result != UserRegistrationResult.OK)
        {
            throw new InvalidOperationException($"Failed to create built-in user with login { login }");
        }
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}