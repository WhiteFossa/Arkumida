using Microsoft.Extensions.Options;
using webapi.Constants;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.Settings;
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
            var accountsService = scope.ServiceProvider.GetRequiredService<IAccountsService>();
            var importerUserSettings = scope.ServiceProvider.GetRequiredService<IOptions<ImporterUserSettings>>().Value;
            
            // Creating roles
            var rolesToCreate = new [] { RolesConstants.UserRole, RolesConstants.ModeratorRole, RolesConstants.AdministratorRole, RolesConstants.ImporterRole };
            foreach (var roleToCreate in rolesToCreate)
            {
                await CreateRoleIfNotExistAsync(accountsService, roleToCreate);
            }
            
            
            // Creating user for importer
            await CreateUserIfNotExistAsync(accountsService, importerUserSettings.Login, string.Empty, importerUserSettings.Password);
        }
    }

    private async Task CreateUserIfNotExistAsync(IAccountsService accountsService, string login, string email, string password)
    {
        if (await accountsService.IsUserExistByLoginAsync(login))
        {
            // Already exist
            return;
        }

        var registrationResult = await accountsService.RegisterUserAsync(new RegistrationDataDto() { Login = login, Email = email, Password = password }, false);
        if (registrationResult.Result != UserRegistrationResult.OK)
        {
            throw new InvalidOperationException($"Failed to create built-in user with login { login }");
        }
    }

    private async Task CreateRoleIfNotExistAsync(IAccountsService accountsService, string roleName)
    {
        if (await accountsService.IsRoleExistsAsync(roleName))
        {
            return;
        }

        await accountsService.CreateRoleAsync(roleName);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}