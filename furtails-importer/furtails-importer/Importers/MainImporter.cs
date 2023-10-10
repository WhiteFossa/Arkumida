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
    public const int TextsImportParallelismDegree = 10;

    public async Task ImportAsync()
    {
        await using var connection = new MySqlConnection(ConnectionString);
        using (var httpClient = await LoginHelper.LogInAsUserAsync(Login, Password))
        {
            // Importing users
            var usersImporter = new UsersImporter(connection, httpClient);
            await usersImporter.ImportAsync();

            // Importing tags
            var tagsImporter = new TagsImporter(connection, httpClient);
            await tagsImporter.Import();
            
            // Importing texts
            var textsImporter = new TextsImporter(connection, httpClient, usersImporter);
            await textsImporter.Import();
            
            // Importing private messages
            var privateMessagesImporter = new PrivateMessagesImporter(connection, httpClient, usersImporter);
            await privateMessagesImporter.ImportAsync();
        }
    }
}