using Dapper;
using MySqlConnector;

namespace furtails_importer.Importers;

/// <summary>
/// Main importer, it calls secondary importets
/// </summary>
public class MainImporter
{
    public const string ConnectionString = "Server=localhost; User ID=root; Password=504Pi5gFfUlfCcoiBTuT3IHXViPzEq; Database=furtails";
    
    public const string BaseUrl = @"http://localhost:5220/api/";

    public async Task ImportAsync()
    {
        await using (var connection = new MySqlConnection(ConnectionString))
        using (var httpClient = new HttpClient())
        {
            // Importing tags
            var tagsImporter = new TagsImporter(connection, httpClient);

            await tagsImporter.Import();
        }
    }
}