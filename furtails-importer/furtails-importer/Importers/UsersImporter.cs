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

    public async Task ImportAsync()
    {
        var users = _connection.Query<FtUser>
            (
                @"select
                    id as Id,
                    username as Username,
                    email as Email,
                    avatar as AvatarType,
                    info as About
                from ft_users
                order by registered desc"
            )
            .ToList();

        foreach (var user in users)
        {
            #region Creature data fixup

            if (user.About == null)
            {
                user.About = String.Empty;
            }
            
            #endregion
            
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
                Password = GeneratePassword()
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

            await RegisterUserAsync(registrationData);
            
            Console.WriteLine("Starting to edit profile...");

            using (var userHttpClient = await LoginHelper.LogInAsUserAsync(registrationData.Login, registrationData.Password))
            {
                // User info
                var currentLoggedInUser = await LoginHelper.GetCurrentLoggedInUserInfoAsync(userHttpClient);
                
                // Do user have an avatar?
                var avatarPath = GetAvatarPath(user);
                if (avatarPath != string.Empty)
                {
                    if (File.Exists(avatarPath))
                    {
                        // Yes, shi have
                        Console.WriteLine("Uploading avatar...");
                        
                        var avatarContent = await File.ReadAllBytesAsync(avatarPath);
                    
                        // Uploading
                        var uploadedAvatarFile = await FilesHelper.UploadFileToArkumidaAsync(userHttpClient, Path.GetFileName(avatarPath), GetMimeTypeByAvatarType(user.AvatarType), avatarContent);
                    
                        // Creating
                        var avatarToCreate = new AvatarDto()
                        {
                            Name = "Аватара по-умолчанию",
                            FileId = uploadedAvatarFile.FileInfo.Id
                        };

                        var createdAvatar = await CreateAvatarAsync(userHttpClient, currentLoggedInUser.Id, avatarToCreate);

                        // Setting as default
                        await SetCurrentAvatarAsync(userHttpClient, currentLoggedInUser.Id, createdAvatar.Id);
                    }
                }
                
                // Editing about info
                Console.WriteLine("Editing about...");
                await UpdateAboutInfoAsync(userHttpClient, currentLoggedInUser.Id, user.About);
            }

            Console.WriteLine("Done");
        }
    }

    public string GenerateNonexistentEmail()
    {
        return $"nonexistent-{Guid.NewGuid()}@example.com";
    }

    public string GeneratePassword()
    {
        return SHA512Helper.CalculateSHA512(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())); // TODO: Is it secure? Seems so;
    }
    
    public async Task<RegistrationResultDto> RegisterUserAsync(RegistrationDataDto registrationData)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/Register", new UserRegistrationRequest() { RegistrationData = registrationData});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<UserRegistrationResponse>(await response.Content.ReadAsStringAsync());

        if (responseData.RegistrationResult.Result != UserRegistrationResult.OK)
        {
            Console.WriteLine("Failed to register user!");
            Console.WriteLine($"Login: { registrationData.Login }");
            Console.WriteLine($"Email: { registrationData.Email }");
            Console.WriteLine($"Reason: { responseData.RegistrationResult.Result }");
            
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
    
    public async Task<CreatureDto> FindCreatureByLogin(string login)
    {
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/FindByLogin", new FindCreatureByLoginRequest() { SearchData = new FindCreatureByLoginDto() { Login = login }});
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        var responseData = JsonSerializer.Deserialize<FindCreatureByLoginResponse>(await response.Content.ReadAsStringAsync());
        if (!responseData.IsFound)
        {
            return null;
        }

        return responseData.Creature;
    }

    private string GetAvatarPath(FtUser user)
    {
        string ext;

        switch (user.AvatarType)
        {
            case 0:
                return ""; // No avatar
            
            case 1:
                ext = "gif";
                break;
            
            case 2:
                ext = "jpg";
                break;
            
            case 3:
                ext = "png";
                break;
            
            default:
                throw new ArgumentException($"Incorrect avatar type: {user.AvatarType}");
        }
        
        return $"{ MainImporter.UsersDbRoot }{user.Id}/orig.{ext}";
    }
    
    private string GetMimeTypeByAvatarType(int avatarType)
    {
        switch (avatarType)
        {
            case 1:
                return "image/gif";

            case 2:
                return "image/jpeg";
            
            case 3:
                return "image/png";
                
            default:
                throw new ArgumentException("Wrong subtype!", nameof(avatarType));
        }
    }
    
    public async Task<AvatarDto> CreateAvatarAsync(HttpClient client, Guid creatureId, AvatarDto avatar)
    {
        var response = await client.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/{ creatureId }/CreateAvatar", new CreateAvatarRequest() { Avatar = avatar });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreateAvatarResponse>(await response.Content.ReadAsStringAsync());

        return responseData.Avatar;
    }
    
    public async Task<CreatureWithProfileDto> SetCurrentAvatarAsync(HttpClient client, Guid creatureId, Guid avatarId)
    {
        var response = await client.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/{ creatureId }/SetCurrentAvatar", new SetCurrentAvatarRequest() { AvatarId = avatarId });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreatureWithProfileResponse>(await response.Content.ReadAsStringAsync());

        return responseData.CreatureWithProfile;
    }

    public async Task<CreatureWithProfileDto> UpdateAboutInfoAsync(HttpClient client, Guid creatureId, string newAbout)
    {
        var response = await client.PostAsJsonAsync($"{MainImporter.BaseUrl}Users/{ creatureId }/About/Update", new UpdateAboutInfoRequest() { NewAbout = newAbout });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
            
        var responseData = JsonSerializer.Deserialize<CreatureWithProfileResponse>(await response.Content.ReadAsStringAsync());

        return responseData.CreatureWithProfile;
    }
}