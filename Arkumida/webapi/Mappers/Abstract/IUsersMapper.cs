using webapi.Dao.Models;
using webapi.Models;

namespace webapi.Mappers.Abstract;

/// <summary>
/// Mapper to map users
/// </summary>
public interface IUsersMapper
{
    IReadOnlyCollection<User> Map(IEnumerable<UserDbo> users);

    User Map(UserDbo user);

    UserDbo Map(User user);

    IReadOnlyCollection<UserDbo> Map(IEnumerable<User> users);
}