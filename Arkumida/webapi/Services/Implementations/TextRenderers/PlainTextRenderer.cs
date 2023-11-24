#region License
// Arkumida - Furtails.pw next generation backend
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Text;
using Microsoft.Extensions.Options;
using webapi.Models;
using webapi.Models.Api.DTOs;
using webapi.Models.Enums;
using webapi.Models.Settings;
using webapi.Services.Abstract.TextRenderers;

namespace webapi.Services.Implementations.TextRenderers;

public class PlainTextRenderer : IPlainTextRenderer
{
    private readonly SiteInfoSettings _siteInfoSettings;

    public PlainTextRenderer
    (
        IOptions<SiteInfoSettings> siteInfoSettings
    )
    {
        _siteInfoSettings = siteInfoSettings.Value;
    }
    
    public async Task<string> RenderAsync(Text textMetadata, IReadOnlyCollection<TextElementDto> textElements)
    {
        var sb = new StringBuilder();

        // Header
        sb.Append(await RenderTextMetadataAsync(textMetadata));
        
        foreach (var textElement in textElements)
        {
            sb.Append(RenderTextElement(textElement));
        }
        
        return sb.ToString().Trim();
    }

    private async Task<string> RenderTextMetadataAsync(Text textMetadata)
    {
        // Authors
        var authors = string.Join
        (
            Environment.NewLine,
            
            textMetadata
                .Authors
                .Select(a => CreatureToText(a))
        );
        
        // Translators
        var translatorsSb = new StringBuilder();
        if (textMetadata.Translators.Any())
        {
            translatorsSb.AppendLine();
            translatorsSb.AppendLine();
            translatorsSb.AppendLine("Переводчик(и):");

            translatorsSb.Append(string.Join
                (
                    Environment.NewLine,
                    
                    textMetadata
                        .Translators
                        .Select(t => CreatureToText(t))
                ));
        }
        
        // Categories
        var categories = string.Join
        (
            ", ",

            textMetadata
                .Tags
                .Where(t => t.IsCategory)
                .Select(t => t.Name)
        );
        
        // Tags
        var tags = string.Join
        (
            ", ",

            textMetadata
                .Tags
                .Where(t => !t.IsCategory)
                .Select(t => t.Name)
        );
        
        return$@"--------------------------------------------------------------------------------
Название:
{textMetadata.Title}

Загрузил:
{CreatureToText(textMetadata.Publisher)}

Автор(ы):
{authors}{translatorsSb}

Категории:
{categories}

Теги:
{tags}

Краткое описание:
{textMetadata.Description}

Ссылка:
{_siteInfoSettings.BaseUrl}/texts/{textMetadata.Id}/page/1
--------------------------------------------------------------------------------

";
    }

    private string CreatureToText(CreatureWithProfile creatureWithProfile)
    {
        return $"{ creatureWithProfile.DisplayName } <{ creatureWithProfile.Email }>";
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