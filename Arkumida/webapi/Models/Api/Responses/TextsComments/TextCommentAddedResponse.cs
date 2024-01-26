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

using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs.TextsComments;

namespace webapi.Models.Api.Responses.TextsComments;

/// <summary>
/// Response with added (or imported) text comment
/// </summary>
public class TextCommentAddedResponse
{
    /// <summary>
    /// Comment
    /// </summary>
    [JsonPropertyName("comment")]
    public TextCommentDto Comment { get; }

    public TextCommentAddedResponse
    (
        TextCommentDto comment
    )
    {
        Comment = comment ?? throw new ArgumentNullException("Comment must not be null!", nameof(comment));
    }
}