namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with private messages
/// </summary>
public interface IPrivateMessagesService
{
    /// <summary>
    /// Get count of unread private messages for given creature
    /// </summary>
    Task<int> GetUnreadPrivateMessagesCountAsync(Guid creatureId);
}