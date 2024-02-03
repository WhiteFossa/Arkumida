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

public class FtForum
{
    public int Id { get; set; }

    public string ForumName { get; set; }

    public string ForumDescription { get; set; }

    public string RedirectUrl { get; set; }

    public string Moderators { get; set; }

    public int TopicsCount { get; set; }

    public int PostsCount { get; set; }

    public long LastPostTimestamp { get; set; }

    public int LastPostId { get; set; }

    public string LastPoster { get; set; }

    public int SortBy { get; set; }

    public int DisplayPosition { get; set; }

    public int ForumCategoryId { get; set; }

}