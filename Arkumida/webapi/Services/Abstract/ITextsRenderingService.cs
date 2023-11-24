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

using webapi.Dao.Models.Enums.RenderedTexts;
using webapi.Models;

namespace webapi.Services.Abstract;

/// <summary>
/// Service to render texts / get rendered texts
/// </summary>
public interface ITextsRenderingService
{
    /// <summary>
    /// Render text to file of given type
    /// </summary>
    Task<byte[]> RenderTextToFileContentAsync(Text metadata, RenderedTextType type);

    /// <summary>
    /// Renders text to a string (only content is rendered, no header, no title and so on). Use it for indexing to OpenSearch
    /// </summary>
    Task<string> RenderTextContentToString(Text metadata);

    /// <summary>
    /// Get rendered text. Will return null if rendered text doesn't exist (in this case you probably want to render a text and put it to DB)
    /// </summary>
    Task<RenderedText> GetRenderedTextAsync(Guid textId, RenderedTextType type);

    /// <summary>
    /// Renders text (using RenderTextToFileContentAsync()) and puts it into database
    /// </summary>
    Task<RenderedText> RenderTextToDbAsync(Guid textId, RenderedTextType type);
}