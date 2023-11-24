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

namespace webapi.Models.Api.Responses;

/// <summary>
/// Information about how to help Redgerra
/// </summary>
public class DonateToRegderraInfoResponse
{
    /// <summary>
    /// Donate page URL
    /// </summary>
    [JsonPropertyName("donatePageUrl")]
    public string DonatePageUrl { get; }
    
    /// <summary>
    /// Donate card number
    /// </summary>
    [JsonPropertyName("donateCardNumber")]
    public string DonateCardNumber { get; }

    public DonateToRegderraInfoResponse
    (
        string donatePageUrl,
        string donateCardNumber
    )
    {
        if (string.IsNullOrWhiteSpace(donatePageUrl))
        {
            throw new ArgumentException("Donate page URL must be populated.", nameof(donatePageUrl));
        }
        DonatePageUrl = donatePageUrl;

        if (string.IsNullOrWhiteSpace(donateCardNumber))
        {
            throw new ArgumentException("Donate card number must be populated.", nameof(donateCardNumber));
        }
        DonateCardNumber = donateCardNumber;
    }
}