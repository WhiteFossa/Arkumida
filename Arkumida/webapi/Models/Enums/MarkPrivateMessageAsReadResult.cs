namespace webapi.Models.Enums;

/// <summary>
/// Possible result of marking message as read. There is no need to process cases like "message not found" - they are processed as exceptions
/// </summary>
public enum MarkPrivateMessageAsReadResult
{
    Successful = 0,
    
    AlreadyMarkedAsRead = 1
}