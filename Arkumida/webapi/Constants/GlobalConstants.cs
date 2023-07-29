namespace webapi.Constants;

/// <summary>
/// Store global constants here
/// </summary>
public static class GlobalConstants
{
    #region Security
    
    /// <summary>
    /// Take JWT secret from this setting in appsettings.json
    /// </summary>
    public const string JwtSecretSettingName = "JWT:Secret";
    
    /// <summary>
    /// Take JWT valid issuer from this setting in appsettings.json
    /// </summary>
    public const string JwtValidIssuerSettingName = "JWT:ValidIssuer";
    
    /// <summary>
    /// Take JWT valid audience from this setting in appsettings.json
    /// </summary>
    public const string JwtValidAudienceSettingName = "JWT:ValidAudience";
    
    /// <summary>
    /// Take JWT lifetime from this setting in appsettings.json
    /// </summary>
    public const string JwtLifetimeSettingName = "JWT:Lifetime";
    
    #endregion
    
    #region Importer user

    /// <summary>
    /// Take importer user login from this setting in appsettings.json
    /// </summary>
    public const string ImporterUserLoginSettingName = "ImporterUser:Login";

    /// <summary>
    /// Take importer user email from this setting in appsettings.json
    /// </summary>
    public const string ImporterUserEmailSettingName = "ImporterUser:Email";
    
    /// <summary>
    /// Take importer user password from this setting in appsettings.json
    /// </summary>
    public const string ImporterUserPasswordSettingName = "ImporterUser:Login";
    
    #endregion
}