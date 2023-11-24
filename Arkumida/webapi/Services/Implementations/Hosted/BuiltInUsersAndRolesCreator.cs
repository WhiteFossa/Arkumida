#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

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

            // Adding Importer to Importer and User roles
            var importer = await accountsService.FindUserByLoginAsync(importerUserSettings.Login);
            await AddCreatureToRoleIfNotInRoleAsync(accountsService, importer.Id, RolesConstants.ImporterRole);
            await AddCreatureToRoleIfNotInRoleAsync(accountsService, importer.Id, RolesConstants.UserRole);

            #endregion
        }
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