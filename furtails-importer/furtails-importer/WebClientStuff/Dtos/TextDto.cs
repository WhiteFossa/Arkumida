using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

public class TextDto
{
    /// <summary>
    /// Text ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// When text was created
    /// </summary>
    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// When text was updated last time
    /// </summary>
    [JsonPropertyName("lastUpdateTime")]
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Text title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Text description (for text info)
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Text sections
    /// </summary>
    [JsonPropertyName("sections")]
    public IList<TextSectionDto> Sections { get; set; }
    
    /// <summary>
    /// How many times text was read
    /// </summary>
    [JsonPropertyName("readsCount")]
    public long ReadsCount { get; set; }
    
    /// <summary>
    /// Votes count for text
    /// </summary>
    [JsonPropertyName("votesCount")]
    public long VotesCount { get; set; }

    /// <summary>
    /// Votes pro
    /// </summary>
    [JsonPropertyName("votesPlus")]
    public long VotesPlus { get; set; }

    /// <summary>
    /// Votes contra
    /// </summary>
    [JsonPropertyName("votesMinus")]
    public long VotesMinus { get; set; }
}