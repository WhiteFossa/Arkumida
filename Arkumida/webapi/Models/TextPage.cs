using webapi.Models.Api.DTOs;
using webapi.Services.Abstract;

namespace webapi.Models;

/// <summary>
/// Text page
/// </summary>
public class TextPage
{
    /// <summary>
    /// Page ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Page number, order pages by this value
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// Text sections
    /// </summary>
    public IList<TextSection> Sections { get; set; }

    public TextPageDto ToDto(IReadOnlyCollection<TextFile> textFiles, ITextsService textsService)
    {
        return new TextPageDto(Id, Number, Sections.Select(s => s.ToDto(textFiles, textsService)).ToList());
    }
}