namespace webapi.Models.Settings;

/// <summary>
/// Settings with base site informations
/// </summary>
public class SiteInfoSettings
{
    /// <summary>
    /// Base URL
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Site title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Administrator's email
    /// </summary>
    public string AdminEmail { get; set; }
    
    /// <summary>
    /// Site sources are here
    /// </summary>
    public string SourcesUrl { get; set; }

    /// <summary>
    /// Telegram chat URL
    /// </summary>
    public string TelegramChatUrl { get; set; }

    /// <summary>
    /// Telegram chat name
    /// </summary>
    public string TelegramChatName { get; set; }
}