using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class UsersMapper : IUsersMapper
{
    public IReadOnlyCollection<User> Map(IEnumerable<UserDbo> users)
    {
        if (users == null)
        {
            return null;
        }

        return users.Select(u => Map(u)).ToList();
    }

    public User Map(UserDbo user)
    {
        if (user == null)
        {
            return null;
        }
        
        return new User
        (
            user.Id,
            user.UserName,
            user.Email,
            user.DisplayName
        );
    }

    public UserDbo Map(User user)
    {
        if (user == null)
        {
            return null;
        }
        
        return new UserDbo()
        {
            Id = user.Id,
            UserName = user.Login,
            Email = user.Email,
            DisplayName = user.DisplayName
        };
    }

    public IReadOnlyCollection<UserDbo> Map(IEnumerable<User> users)
    {
        if (users == null)
        {
            return null;
        }

        return users.Select(u => Map(u)).ToList();
    }
}