namespace webapi.Models.Enums;

/// <summary>
/// Password reset initiation result
/// </summary>
public enum PasswordResetInitiationResult
{
    Initiated = 0,
    
    CreatureNotFound = 1,
    
    CreatureHaveNoEmail = 2,
    
    CreatureHaveNoConfirmedEmail = 3,
    
    FailedToSendEmail = 4
}