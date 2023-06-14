using Dapper;
using MySqlConnector;

namespace furtails_importer.Importers;

/// <summary>
/// Main importer, it calls secondary importets
/// </summary>
public class MainImporter
{
    const string connectionString = "Server=localhost; User ID=root; Password=504Pi5gFfUlfCcoiBTuT3IHXViPzEq; Database=furtails";
    
    public async Task ImportAsync()
    {
        await using (var connection = new MySqlConnection(connectionString))
        {
            // Importing tags
            var tagsImporter = new TagsImporter(connection);

            await tagsImporter.Import();
        }
    }
}