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
using webapi.Models.Settings;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Forum;

namespace webapi.Services.Implementations.Hosted;

public class ForumSeeder : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ForumSeeder
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
            #region DI
            
            var forumSettings = scope.ServiceProvider.GetRequiredService<IOptions<ForumSettings>>().Value;
            var importerUserSettings = scope.ServiceProvider.GetRequiredService<IOptions<ImporterUserSettings>>().Value;
            
            var forumService = scope.ServiceProvider.GetRequiredService<IForumService>();
            var accountsService = scope.ServiceProvider.GetRequiredService<IAccountsService>();
            
            #endregion

            #region Creating texts comments section

            if (await forumService.GetSectionByIdAsync(forumSettings.TextsCommentsSectionId) == null)
            {
                var importerCreature = await accountsService.FindUserByLoginAsync(importerUserSettings.Login);
            
                await forumService.CreateSectionAsync
                (
                    forumSettings.TextsCommentsSectionName,
                    forumSettings.TextsCommentsSectionDescription,
                    importerCreature.Id,
                    forumSettings.TextsCommentsSectionId
                );    
            }

            #endregion
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}