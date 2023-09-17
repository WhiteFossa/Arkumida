namespace webapi.Constants;

/// <summary>
/// Store global constants here
/// </summary>
public static class GlobalConstants
{
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
    public const string ImporterUserPasswordSettingName = "ImporterUser:Password";
    
    #endregion
    
    #region Site info
    
    /// <summary>
    /// Take site base URL from this setting in appsettings.json
    /// </summary>
    public const string SiteInfoBaseUrlSettingName = "SiteInfo:BaseUrl";
    
    /// <summary>
    /// Take site title from this setting in appsettings.json
    /// </summary>
    public const string SiteInfoTitleSettingName = "SiteInfo:Title";
    
    #endregion
}