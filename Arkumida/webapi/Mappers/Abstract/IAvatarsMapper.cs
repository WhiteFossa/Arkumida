using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper for avatars
/// </summary>
public interface IAvatarsMapper
{
    IReadOnlyCollection<Avatar> Map(IEnumerable<AvatarDbo> avatars);

    Avatar Map(AvatarDbo avatar);

    AvatarDbo Map(Avatar avatar);

    IReadOnlyCollection<AvatarDbo> Map(IEnumerable<Avatar> avatars);
}