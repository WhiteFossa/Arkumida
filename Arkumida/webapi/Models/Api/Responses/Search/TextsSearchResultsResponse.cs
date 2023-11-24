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
using webapi.Models.Api.DTOs.Search;

namespace webapi.Models.Api.Responses.Search;

/// <summary>
/// Response with texts search results
/// </summary>
public class TextsSearchResultsResponse
{
    /// <summary>
    /// This request was made
    /// </summary>
    [JsonPropertyName("query")]
    public string Query { get; private set; }

    /// <summary>
    /// Found texts
    /// </summary>
    [JsonPropertyName("foundTexts")]
    public IReadOnlyCollection<FoundTextDto> FoundTexts { get; private set; }

    /// <summary>
    /// Total count of found texts (disregarding pagination)
    /// </summary>
    [JsonPropertyName("foundTextsTotalCount")]
    public long FoundTextsTotalCount { get; set; }

    public TextsSearchResultsResponse
    (
        string query,
        IReadOnlyCollection<FoundTextDto> foundTexts,
        long foundTextsTotalCount
    )
    {
        // Query string may be empty, however we will not return any text in this case
        Query = query ?? throw new ArgumentNullException(nameof(query), "Query must not be null!");

        FoundTexts = foundTexts ?? throw new ArgumentNullException(nameof(foundTexts), "Found texts must not be null!");

        if (foundTextsTotalCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(foundTextsTotalCount), foundTextsTotalCount, "Found texts total count must not be negative!");
        }
        FoundTextsTotalCount = foundTextsTotalCount;
    }
}