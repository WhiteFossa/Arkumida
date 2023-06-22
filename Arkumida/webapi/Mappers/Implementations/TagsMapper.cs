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