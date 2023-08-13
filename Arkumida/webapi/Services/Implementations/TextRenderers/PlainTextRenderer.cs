using System.Text;
using webapi.Constants;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Services.Abstract;
using webapi.Services.Abstract.TextRenderers;

namespace webapi.Services.Implementations.TextRenderers;

public class PlainTextRenderer : IPlainTextRenderer
{
    private readonly IConfigurationService _configurationService;

    public PlainTextRenderer
    (
        IConfigurationService configurationService
    )
    {
        _configurationService = configurationService;
    }
    
    public async Task<string> RenderAsync(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements)
    {
        var sb = new StringBuilder();

        // Header
        sb.Append(await RenderTextMetadataAsync(textMetadata));

        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        
        foreach (var textElement in textElements)
        {
            sb.Append(RenderTextElement(textElement));
        }
        
        return sb.ToString().Trim();
    }

    private async Task<string> RenderTextMetadataAsync(Text textMetadata)
    {
        var sb = new StringBuilder();

        // Title
        sb.AppendLine($"Название: { textMetadata.Title }");
        sb.AppendLine();

        // Publisher
        sb.AppendLine($"Загрузил:");
        sb.AppendLine(CreatureToText(textMetadata.Publisher));
        sb.AppendLine();

        // Authors
        sb.AppendLine("Автор(ы):");
        foreach (var author in textMetadata.Authors)
        {
            sb.AppendLine(CreatureToText(author));
        }
        sb.AppendLine();
        
        // Translators
        if (textMetadata.Translators.Any())
        {
            sb.AppendLine("Переводчик(и):");
            foreach (var translator in textMetadata.Translators)
            {
                sb.AppendLine(CreatureToText(translator));
            }
            sb.AppendLine();
        }
        
        // Categories
        sb.Append("Категории: ");
        sb.Append(string.Join
        (
            ", ",

            textMetadata
                .Tags
                .Where(t => t.IsCategory)
                .Select(t => t.Name)
        ));
        sb.AppendLine();
        sb.AppendLine();
        
        // Tags
        sb.Append("Теги: ");
        sb.Append(string.Join
        (
            ", ",

            textMetadata
                .Tags
                .Where(t => !t.IsCategory)
                .Select(t => t.Name)
        ));
        sb.AppendLine();
        sb.AppendLine();
        
        // Annotation
        sb.AppendLine("Краткое описание:");
        sb.AppendLine("---");
        sb.AppendLine(textMetadata.Description);
        sb.AppendLine("---");
        
        // Permalink
        var baseUrl = await _configurationService.GetConfigurationStringAsync(GlobalConstants.SiteInfoBaseUrlSettingName);
        sb.AppendLine($"Ссылка: {baseUrl}/texts/{textMetadata.Id}/page/1");
        sb.AppendLine();
        
        return sb.ToString();
    }

    private string CreatureToText(Creature creature)
    {
        return $"{ creature.DisplayName } <{ creature.Email }>";
    }
    
    private string RenderTextElement(TextElementDto textElement)
    {
        if (textElement.Type == TextElementType.ParagraphBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.PlainText)
        {
            return textElement.Content;
        }

        if (textElement.Type == TextElementType.ParagraphEnd)
        {
            return Environment.NewLine;
        }

        if (textElement.Type == TextElementType.FullWidthAlignedTextBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.FullWidthAlignedTextEnd)
        {
            return Environment.NewLine;
        }

        if (textElement.Type == TextElementType.ItalicBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.ItalicEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.BoldBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.BoldEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.UnderlineBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.UnderlineEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.StrikeOutBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.StrikeOutEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.CentrallyAlignedTextBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.CentrallyAlignedTextEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.LeftAlignedTextBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.LeftAlignedTextEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.RightAlignedTextBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.RightAlignedTextEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.TitleBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.TitleEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.PreformattedTextBegin)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.PreformattedTextEnd)
        {
            return string.Empty;
        }

        if (textElement.Type == TextElementType.QuoteBegin)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.QuoteEnd)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.AsciiArtBegin)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.AsciiArtEnd)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.UrlBegin)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.UrlEnd)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.ColorBegin)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.ColorEnd)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.SizedAsciiArtBegin)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.SizedAsciiArtEnd)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.EmbeddedImage)
        {
            return string.Empty;
        }
        
        if (textElement.Type == TextElementType.ComicsImage)
        {
            return string.Empty;
        }

        throw new ArgumentException("Unknown text element!", nameof(textElement.Type));
    }
}