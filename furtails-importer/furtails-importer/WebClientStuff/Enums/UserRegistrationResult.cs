namespace furtails_importer.WebClientStuff.Enums;

/// <summary>
/// User registration result enum
/// </summary>
public enum UserRegistrationResult
{
    OK = 0,
    
    LoginIsTaken = 1,
    
    WeakPassword = 2,

    GenericError = 4
}