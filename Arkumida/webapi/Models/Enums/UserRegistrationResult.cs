namespace webapi.Models.Enums;

/// <summary>
/// User registration result enum
/// </summary>
public enum UserRegistrationResult
{
    OK = 0,
    
    LoginIsTaken = 1,
    
    WeakPassword = 2,
    
    EmailIsTaken = 3,

    GenericError = 4
}