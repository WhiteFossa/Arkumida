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
using webapi.Models.Api.DTOs.PrivateMessages;

namespace webapi.Models.Api.Responses.PrivateMessages;

/// <summary>
/// Response on private message sending
/// </summary>
public class SentPrivateMessageResponse
{
    /// <summary>
    /// Is message sent successfully
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; }

    /// <summary>
    /// Sent message
    /// </summary>
    [JsonPropertyName("message")]
    public PrivateMessageDto Message { get; }

    public SentPrivateMessageResponse
    (
        bool isSuccessful,
        PrivateMessageDto message
    )
    {
        IsSuccessful = isSuccessful;
        Message = message ?? throw new ArgumentNullException(nameof(message), "Sent message must not be null!");
    }
}