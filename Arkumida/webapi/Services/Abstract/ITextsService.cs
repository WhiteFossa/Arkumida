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

using webapi.Dao.Models.Enums;
using webapi.Models;
using webapi.Models.Api.DTOs;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to work with texts
/// </summary>
public interface ITextsService
{
    /// <summary>
    /// Create new text. Returns created text, which is different from input one (for example it have ID and users populated)
    /// </summary>
    Task<Text> CreateTextAsync(Text text);

    /// <summary>
    /// Get some texts metadata from database
    /// </summary>
    Task<IReadOnlyCollection<TextInfoDto>> GetTextsInfosAsync(TextOrderMode orderMode, int skip, int take);

    /// <summary>
    /// Get text metadata by text ID
    /// </summary>
    Task<TextInfoDto> GetTextInfoByIdAsync(Guid textId);

    /// <summary>
    /// Get total texts count
    /// </summary>
    Task<int> GetTotalTextsCountAsync();

    /// <summary>
    /// Get last text add time
    /// </summary>
    Task<DateTime> GetLastTextAddTimeAsync();

    /// <summary>
    /// Get text metadata, required to display read page (actual text is NOT returned, for text see GetTextPageAsync()) 
    /// </summary>
    Task<TextReadDto> GetTextToReadAsync(Guid textId);

    /// <summary>
    /// Get text pages count
    /// </summary>
    Task<int> GetTextPagesCountAsync(Guid textId);
    
    /// <summary>
    /// Get text page data (actual text returned here)
    /// </summary>
    Task<TextPageDto> GetTextPageAsync(Guid textId, int pageNumber);

    /// <summary>
    /// Order text sections such a way, that they will compose a full text
    /// </summary>
    IReadOnlyCollection<TextSection> OrderTextSections(IEnumerable<TextSection> sections);

    /// <summary>
    /// Add existing file to given text under provided name
    /// </summary>
    Task AddFileToTextAsync(Guid textId, string fileName, Guid existingFileId);
    
    #region Comments
    
    
    #endregion
}