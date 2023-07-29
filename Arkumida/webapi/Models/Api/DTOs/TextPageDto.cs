using System.Text.Json.Serialization;

namespace webapi.Models.Api.DTOs;

/// <summary>
/// Text page DTO
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

    public TextPageDto()
    {

    }

    public TextPageDto
    (
        Guid id,
        int number,
        IReadOnlyCollection<TextSectionDto> sections)
    {
        Id = id;
        Number = number;
        Sections = (sections ?? throw new ArgumentNullException(nameof(sections), "Sections must be populated!")).ToList();
    }

    public TextPage ToTextPage()
    {
        return new TextPage()
        {
            Id = Id,
            Number = Number,
            Sections = Sections.Select(s => s.ToTextSection()).ToList()
        };
    }

}