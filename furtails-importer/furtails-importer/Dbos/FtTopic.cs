#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
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

namespace furtails_importer.Dbos;

public class FtTopic
{
    public int Id { get; set; }

    public string Poster { get; set; }

    public string Subject { get; set; }

    public long PostedTimestamp { get; set; }

    public int FirstPostId { get; set; }

    public long LastPostTimestamp { get; set; }

    public int LastPostId { get; set; }

    public string LastPoster { get; set; }

    public int ViewsCount { get; set; }

    public int RepliesCount { get; set; }

    public bool IsClosed { get; set; }

    public bool IsSticky { get; set; }

    public int MovedTo { get; set; }

    public int ForumId { get; set; }
}