using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.PrivateMessages;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Response with all creature's conversations
/// </summary>
public class ConversationsSummariesResponse
{
    /// <summary>
    /// Conversations summaries
    /// </summary>
    [JsonPropertyName("conversationsSummaries")]
    public IReadOnlyCollection<ConversationSummaryDto> ConversationsSummaries { get; }

    public ConversationsSummariesResponse
    (
        IReadOnlyCollection<ConversationSummaryDto> conversationsSummaries
    )
    {
        ConversationsSummaries = conversationsSummaries ?? throw new ArgumentNullException(nameof(conversationsSummaries), "Conversations summaries can't be null!");
    }
}