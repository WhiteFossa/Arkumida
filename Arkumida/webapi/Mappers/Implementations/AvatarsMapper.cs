using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class AvatarsMapper : IAvatarsMapper
{
    private readonly IFilesMapper _filesMapper;

    public AvatarsMapper
    (
        IFilesMapper filesMapper
    )
    {
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<Avatar> Map(IEnumerable<AvatarDbo> avatars)
    {
        if (avatars == null)
        {
            return null;
        }

        return avatars.Select(a => Map(a)).ToList();
    }

    public Avatar Map(AvatarDbo avatar)
    {
        if (avatar == null)
        {
            return null;
        }

        return new Avatar()
        {
            Id = avatar.Id,
            Name = avatar.Name,
            IsCurrent = avatar.IsCurrent,
            UploadTime = avatar.UploadTime,
            File = _filesMapper.Map(avatar.File) 
        };
    }

    public AvatarDbo Map(Avatar avatar)
    {
        if (avatar == null)
        {
            return null;
        }
        
        return new AvatarDbo()
        {
            Id  = avatar.Id,
            Name = avatar.Name,
            IsCurrent = avatar.IsCurrent,
            UploadTime = avatar.UploadTime,
            File = _filesMapper.Map(avatar.File)
        };
    }

    public IReadOnlyCollection<AvatarDbo> Map(IEnumerable<Avatar> avatars)
    {
        if (avatars == null)
        {
            return null;
        }

        return avatars.Select(a => Map(a)).ToList();
    }
}