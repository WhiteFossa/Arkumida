using Microsoft.Net.Http.Headers;

namespace webapi.Helpers;

/// <summary>
/// Helper to get user agent
/// </summary>
public static class UserAgentHelper
{
    /// <summary>
    /// Gets user agent or returns "User agent header not found"
    /// </summary>
    public static string GetUserAgent(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgent))
        {
            return userAgent.ToString();
        }
        
        return "User agent header not found";
    }
}