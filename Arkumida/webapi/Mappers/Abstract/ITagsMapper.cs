using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for tags
/// </summary>
public interface ITagsMapper
{
    IReadOnlyCollection<Tag> Map(IEnumerable<TagDbo> tags);

    Tag Map(TagDbo tag);

    TagDbo Map(Tag tag);

    IReadOnlyCollection<TagDbo> Map(IEnumerable<Tag> tags);
}