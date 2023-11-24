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
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// List of text infos
/// </summary>
public class TextsInfosListResponse
{
    /// <summary>
    /// Text infos
    /// </summary>
    [JsonPropertyName("textInfos")]
    public IReadOnlyCollection<TextInfoDto> TextInfos { get; private set; }

    /// <summary>
    /// How many texts remaining (total count - (skip + take))
    /// </summary>
    [JsonPropertyName("remainingTexts")]
    public int RemainingTexts { get; private set; }

    public TextsInfosListResponse
    (
        IReadOnlyCollection<TextInfoDto> textInfos,
        int remainingTexts
    )
    {
        TextInfos = textInfos ?? throw new ArgumentNullException(nameof(textInfos), "Text infos must not be null");

        if (remainingTexts < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(remainingTexts), "Remaining texts count must be non-negative");
        }

        RemainingTexts = remainingTexts;
    }
}