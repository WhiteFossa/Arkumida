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
/// Forum settings (read from appsettings.json)
/// </summary>
public class ForumSettings
{
    /// <summary>
    /// Text comments section ID
    /// </summary>
    public Guid TextsCommentsSectionId { get; set; }
    
    /// <summary>
    /// Texts comments section name
    /// </summary>
    public string TextsCommentsSectionName { get; set; }
    
    /// <summary>
    /// Texts comments section description
    /// </summary>
    public string TextsCommentsSectionDescription { get; set; }

    /// <summary>
    /// Text comments topic name template
    /// </summary>
    public string TextCommentsTopicNameTemplate { get; set; }

    /// <summary>
    /// Text comments topic description template
    /// </summary>
    public string TextCommentsTopicDescriptionTemplate { get; set; }
}