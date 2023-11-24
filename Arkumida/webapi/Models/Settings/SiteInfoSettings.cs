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

namespace webapi.Models.Settings;

/// <summary>
/// Settings with base site informations
/// </summary>
public class SiteInfoSettings
{
    /// <summary>
    /// Base URL
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Site title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Administrator's email
    /// </summary>
    public string AdminEmail { get; set; }
    
    /// <summary>
    /// Site sources are here
    /// </summary>
    public string SourcesUrl { get; set; }

    /// <summary>
    /// Telegram chat URL
    /// </summary>
    public string TelegramChatUrl { get; set; }

    /// <summary>
    /// Telegram chat name
    /// </summary>
    public string TelegramChatName { get; set; }
}