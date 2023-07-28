using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Dtos;

/// <summary>
/// Text page
/// </summary>
public class TextPageDto
{
    /// <summary>
    /// Page ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Page number, order pages by this value
    /// </summary>
    [JsonPropertyName("number")]
    public int Number { get; set; }

    /// <summary>
    /// Text sections
    /// </summary>
    [JsonPropertyName("sections")]
    public IList<TextSectionDto> Sections { get; set; }
}