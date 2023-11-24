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
        httpClient.Timeout = TimeSpan.FromMinutes(5);
        
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