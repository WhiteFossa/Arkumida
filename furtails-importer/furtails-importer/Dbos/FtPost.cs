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

public class FtPost
{
    public int Id { get; set; }

    public string Poster { get; set; }

    public int PosterId { get; set; }

    public string PosterIp { get; set; }

    public string PosterEmail { get; set; }

    public string Message { get; set; }

    public bool IsHideSmiles { get; set; }

    public long PostedTimestamp { get; set; }

    public long? EditedTimestamp { get; set; }

    public string EditedBy { get; set; }

    public int TopicId { get; set; }
}