using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.Helpers;
using furtails_importer.WebClientStuff.Dtos;
using furtails_importer.WebClientStuff.Enums;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;
using MySqlConnector;

namespace furtails_importer.Importers;

public class UsersImporter
{
    private readonly MySqlConnection _connection;
    private readonly HttpClient _httpClient;

    public UsersImporter(MySqlConnection connection, HttpClient httpClient)
    {
        _connection = connection;
        _httpClient = httpClient;
    }

    public async Task<Dictionary<int, Guid>> ImportAsync()
    {
        var result = new Dictionary<int, Guid>();
        
        var users = _connection.Query<FtUser>
            (
                @"select
                    id as Id,
                    username as Username,
                    email as Email
                from ft_users
                order by registered desc"
            )
            .ToList();

        foreach (var user in users)
        {
            // User may have no email set, we must generate UNIQUE nonexistent email
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = GenerateNonexistentEmail();
            }
            
            Console.WriteLine($"Registering: { user.Username } <{ user.Email }>... ");

            var registrationData = new RegistrationDataDto()
            {
                Login = user.Username,
                Email = user.Email,
                Password = SHA512Helper.CalculateSHA512(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())) // TODO: Is it secure? Seems so
            };

            if (await IsLoginTakenAsync(registrationData.Login))
            {
                // Login is taken, adding postfix
                Console.WriteLine($"Login { registrationData.Login } is taken, adding postfix...");
                    
                var postfixCount = 0;
                string newLogin;

                do
                {
                    newLogin = $"{ registrationData.Login }_{ postfixCount }";
                    
                    Console.WriteLine(newLogin);
                    
                    postfixCount++;
                } while (await IsLoginTakenAsync(newLogin));
                
                registrationData.Login = newLogin;
            }

            if (await IsEmailTakenAsync(registrationData.Email))
            {
                // Email is taken, we have to discard it and force user to set a new email
                registrationData.Email = GenerateNonexistentEmail();
            }

            var registrationResult = await RegisterUserAsync(registrationData);
            
            // Adding to mapping
            Console.WriteLine($"{ user.Username }: { user.Id } -> { registrationResult.UserId }");
            result.Add(user.Id, registrationResult.UserId);

            Console.WriteLine("Done");
        }

        return result;
    }

    private string GenerateNonexistentEmail()
    {
        return $"nonexistent-{Guid.NewGuid()}@example.com";
    }
    
    private async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/Register", new UserRegistrationRequest() { RegistrationData = registrationData});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<UserRegistrationResponse>(await response.Content.ReadAsStringAsync());

        if (responseData.RegistrationResult.Result != UserRegistrationResult.OK)
        {
            throw new InvalidOperationException($"Failed to register user { registrationData.Login }");
        }

        return responseData.RegistrationResult;
    }

    private async Task<bool> IsLoginTakenAsync(string login)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/IsLoginTaken", new CheckIfLoginTakenRequest() { CheckData = new CheckIfLoginTakenDto() { Login = login }});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CheckIfLoginTakenResponse>(await response.Content.ReadAsStringAsync());

        return responseData.CheckResult.IsTaken;
    }
    
    private async Task<bool> IsEmailTakenAsync(string email)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/IsEmailTaken", new CheckIfEmailTakenRequest() { CheckData = new CheckIfEmailTakenDto() { Email = email }});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CheckIfEmailTakenResponse>(await response.Content.ReadAsStringAsync());

        return responseData.CheckResult.IsTaken;
    }
}