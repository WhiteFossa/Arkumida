using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using furtails_importer.WebClientStuff.Dtos;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;
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

    public async Task ImportAsync()
    {
        await using (var connection = new MySqlConnection(ConnectionString))
        using (var httpClient = new HttpClient())
        {
            // Authenticating on Arkumida
            var authResponseRaw = await httpClient.PostAsJsonAsync($"{BaseUrl}Users/Login", new LoginRequest() { LoginData = new LoginDto() { Login = Login, Password = Password }});
            if (!authResponseRaw.IsSuccessStatusCode)
            {
                Environment.Exit(1);
            }

            var decodedAuthResponse = JsonSerializer.Deserialize<LoginResponse>(await authResponseRaw.Content.ReadAsStringAsync());
            if (!decodedAuthResponse.LoginResult.IsSuccessful)
            {
                Environment.Exit(2);
            }

            var authToken = decodedAuthResponse.LoginResult.Token;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            // Importing users
            var usersImporter = new UsersImporter(connection, httpClient);
            await usersImporter.Import();

            /*// Importing tags
            var tagsImporter = new TagsImporter(connection, httpClient);
            await tagsImporter.Import();
            
            // Importing texts
            var textsImporter = new TextsImporter(connection, httpClient);
            await textsImporter.Import();*/
        }
    }
}