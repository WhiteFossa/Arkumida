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

using webapi.Dao.Models;
using webapi.Dao.Models.Enums;
using webapi.Models;
using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract;

/// <summary>
/// Utils to work with texts (common code for TextsService, TextsRenderingService and so on)
/// </summary>
public interface ITextUtilsService
{
    /// <summary>
    /// Get raw (i.e. not parsed yet) text from database
    /// </summary>
    Task<string> GetRawTextAsync(Guid textId);
    
    /// <summary>
    /// Parse text (of variant) to text elements
    /// </summary>
    /// <param name="textContent">Plain text to parse</param>
    /// <param name="textFiles">Text files (we need it to generate links to images)</param>
    IReadOnlyCollection<TextElementDto> ParseTextToElements(string textContent, IReadOnlyCollection<TextFile> textFiles);
    
    /// <summary>
    /// Get full text metadata (i.e. with publisher, translators and so on) by text ID
    /// </summary>
    Task<Text> GetTextMetadataAsync(Guid textId);
    
    /// <summary>
    /// Get full text metadata for multiple texts
    /// </summary>
    Task<IReadOnlyCollection<Text>> GetTextsMetadatasAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Load-in some data into text metadata (like full data on authors/translators/publisher)
    /// </summary>
    Task<Text> PopulateTextMetadataAsync(TextDbo metadata);
}