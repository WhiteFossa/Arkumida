#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
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

using System.Collections.Concurrent;
using furtails_importer.Helpers;
using MySqlConnector;

namespace furtails_importer.Importers;

/// <summary>
/// Main importer, it calls secondary importets
/// </summary>
public class MainImporter
{
    public const string ConnectionString = "Server=localhost; User ID=root; Password=504Pi5gFfUlfCcoiBTuT3IHXViPzEq; Database=furtails";
    
    public const string BaseUrl = @"http://localhost:5220/api/";
    //public const string BaseUrl = @"https://api.arkumida.furtails.pw/api/";

    public const string Login = "importer-user";
    public const string Password = "xhYOUrMHr27SdBrRhc1Bhpxc6Wip4F";

    public const string TextsDbRoot = @"/home/fossa/Projects/Arkumida-private/furtails-site/furtails/public/filedb/texts/";
    public const string UsersDbRoot = @"/home/fossa/Projects/Arkumida-private/furtails-site/furtails/public/filedb/users/";

    public const int ParallelismDegree = 10;
    public const int TextsImportParallelismDegree = 4;

    public async Task ImportAsync()
    {
        await using var connection = new MySqlConnection(ConnectionString);
        using (var httpClient = await LoginHelper.LogInAsUserAsync(Login, Password))
        {
            // Importing users
            var creaturesMapping = new ConcurrentDictionary<int, Guid>(); // FT to Arkumida creatures mapping
            
            var usersImporter = new UsersImporter(connection, httpClient);
            await usersImporter.ImportAsync(creaturesMapping);

            // Importing tags
            var tagsImporter = new TagsImporter(connection, httpClient);
            await tagsImporter.Import();
            
            // Forums importer (we need it here for comments import)
            var forumImporter = new ForumImporter(connection, httpClient);
            
            // Importing texts
            var textsImporter = new TextsImporter(connection, httpClient, usersImporter, forumImporter);
            await textsImporter.Import(creaturesMapping);
            
            // Importing private messages
            var privateMessagesImporter = new PrivateMessagesImporter(connection, httpClient, usersImporter);
            await privateMessagesImporter.ImportAsync();
        }
    }
}