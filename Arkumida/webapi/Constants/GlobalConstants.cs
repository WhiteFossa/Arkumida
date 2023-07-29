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
}