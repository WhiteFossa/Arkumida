using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using furtails_importer.Importers;
using furtails_importer.WebClientStuff.Dtos;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;

namespace furtails_importer.Helpers;

/// <summary>
/// Helper for logging in as user
/// </summary>
public static class LoginHelper
{
    /// <summary>
    /// Returns HttpClient, logged as given user
    /// </summary>
    public static async Task<HttpClient> LogInAsUserAsync(string login, string password)
    {
        var httpClient = new HttpClient();
        
        // Authenticating on Arkumida
        var authResponseRaw = await httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/Login", new LoginRequest() { LoginData = new LoginDto() { Login = login, Password = password }});
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

        return httpClient;
    }
    
    /// <summary>
    /// Get information about currently logged in user
    /// </summary>
    public static async Task<CreatureDto> GetCurrentLoggedInUserInfoAsync(HttpClient client)
    {
        var response = await client.GetAsync($"{MainImporter.BaseUrl}Users/Current");
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        var responseData = JsonSerializer.Deserialize<LoggedInCreatureResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Creature;
    }
}