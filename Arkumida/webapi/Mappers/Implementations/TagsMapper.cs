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

using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Models.Enums;

namespace webapi.Mappers.Implementations;

public class TagsMapper : ITagsMapper
{
    public IReadOnlyCollection<Tag> Map(IEnumerable<TagDbo> tags)
    {
        if (tags == null)
        {
            return null;
        }

        return tags.Select(t => Map(t)).ToList();
    }

    public Tag Map(TagDbo tag)
    {
        if (tag == null)
        {
            return null;
        }

        return new Tag()
        {
            Id = tag.Id,
            Subtype = tag.Subtype,
            FurryReadableId = tag.FurryReadableId,
            Name = tag.Name,
            IsCategory = tag.IsCategory,
            CategoryOrder = tag.CategoryOrder,
            TextsCount = 0, // This must be calculated outside of mapper
            CategoryTagType = tag.CategoryType,
            SizeCategory = TagSizeCategory.Cat0, // This must be calculated outside of mapper
            IsHidden = tag.IsHidden,
            Meaning = tag.Meaning
        };
    }

    public TagDbo Map(Tag tag)
    {
        if (tag == null)
        {
            return null;
        }

        return new TagDbo()
        {
            Id = tag.Id,
            FurryReadableId = tag.FurryReadableId,
            Subtype = tag.Subtype,
            Name = tag.Name,
            IsCategory = tag.IsCategory,
            CategoryOrder = tag.CategoryOrder,
            CategoryType = tag.CategoryTagType,
            IsHidden = tag.IsHidden,
            Meaning = tag.Meaning
        };
    }

    public IReadOnlyCollection<TagDbo> Map(IEnumerable<Tag> tags)
    {
        if (tags == null)
        {
            return null;
        }

        return tags.Select(t => Map(t)).ToList();
    }
}