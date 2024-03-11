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

namespace webapi.Models.Forum;

/// <summary>
/// Forum topic
/// </summary>
public class ForumTopic
{
    /// <summary>
    /// Topic ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Topic name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Topic description
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Forum messages
    /// </summary>
    public IList<ForumMessage> Messages { get; set; }

    /// <summary>
    /// If this field is not null, then this topic is comments topic for given text
    /// </summary>
    public Text CommentsForText { get; set; }
}