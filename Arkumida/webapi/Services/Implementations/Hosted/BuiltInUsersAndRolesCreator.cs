using Microsoft.Extensions.Options;
using webapi.Constants;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.Settings;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations.Hosted;

/// <summary>
/// Service for creating built-in users
/// </summary>
public class BuiltInUsersAndRolesCreator : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public BuiltInUsersAndRolesCreator
    (
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Creature> creatures;
        
        using (var scope = _scopeFactory.CreateScope())
        {
            // DI
            var accountsService = scope.ServiceProvider.GetRequiredService<IAccountsService>();
            var importerUserSettings = scope.ServiceProvider.GetRequiredService<IOptions<ImporterUserSettings>>().Value;
            
            #region Creating roles
            
            var rolesToCreate = new [] { RolesConstants.UserRole, RolesConstants.ModeratorRole, RolesConstants.AdministratorRole, RolesConstants.ImporterRole };
            foreach (var roleToCreate in rolesToCreate)
            {
                await CreateRoleIfNotExistAsync(accountsService, roleToCreate);
            }
            
            #endregion

            #region Creating Importer service account

            await CreateCreatureIfNotExistAsync(accountsService, importerUserSettings.Login, string.Empty, importerUserSettings.Password);

            // Adding Importer to Importer role
            var importer = await accountsService.FindUserByLoginAsync(importerUserSettings.Login); // Importer can be created before this code update, so we need to fetch shi
            await AddCreatureToRoleIfNotInRoleAsync(accountsService, importer.Id, RolesConstants.ImporterRole);

            #endregion
            
            // To use when we will give creatures an User role
            creatures = await accountsService.GetAllCreaturesAsync();
        }
        
        // Giving users roles outside the scope - because we will create a lot of scopes in parallel operations
        #region Giving all creatures an User role
            
        ParallelOptions userRoleAddingParallelismLevel = new()
        {
            MaxDegreeOfParallelism = GlobalConstants.AddingUserRoleToCreaturesParallelismLevel
        };
            
        await Parallel.ForEachAsync(creatures, userRoleAddingParallelismLevel, async (creature, token) =>
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var accountsServiceLocal = scope.ServiceProvider.GetRequiredService<IAccountsService>();
                await AddCreatureToRoleIfNotInRoleAsync(accountsServiceLocal, creature.Id, RolesConstants.UserRole);
            }
        });
        
        #endregion
    }

    private async Task AddCreatureToRoleIfNotInRoleAsync(IAccountsService accountsService, Guid creatureId, string roleName)
    {
        if (!await accountsService.IsCreatureInRoleAsync(creatureId, roleName))
        {
            await accountsService.AddCreatureToRoleAsync(creatureId, roleName);
        }
    }
    
    private async Task CreateCreatureIfNotExistAsync(IAccountsService accountsService, string login, string email, string password)
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