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

using webapi.Models.Creatures;

namespace webapi.Models.Forum;

/// <summary>
/// Forum section (they are recursive)
/// </summary>
public class ForumSection
{
    /// <summary>
    /// Topic ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Section name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Section description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Section creation time
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Section author
    /// </summary>
    public CreatureWithProfile Author { get; set; }

    /// <summary>
    /// Subsections of this section
    /// </summary>
    public IList<ForumSection> Subsections { get; set; }

    /// <summary>
    /// Topics in this section
    /// </summary>
    public IList<ForumTopic> Topics { get; set; }
}