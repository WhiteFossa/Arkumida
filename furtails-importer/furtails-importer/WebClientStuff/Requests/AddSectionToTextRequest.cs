using System.Text.Json.Serialization;

namespace furtails_importer.WebClientStuff.Requests;

public class AddSectionToTextRequest
{
    /// <summary>
    /// Add section to this text
    /// </summary>
    [JsonPropertyName("textId")]
    public Guid TextId { get; set; }

    /// <summary>
    /// Add this section to text
    /// </summary>
    [JsonPropertyName("sectionId")]
    public Guid SectionId { get; set; }
}