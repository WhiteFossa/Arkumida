using webapi.Dao.Abstract;
using webapi.Mappers.Abstract;
using webapi.Models;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TextsService : ITextsService
{
    private readonly ITextsDao _textsDao;
    private readonly ITextsMapper _textsMapper;

    public TextsService
    (
        ITextsDao textsDao,
        ITextsMapper textsMapper
    )
    {
        _textsDao = textsDao;
        _textsMapper = textsMapper;
    }
    
    public async Task CreateTextAsync(Text text)
    {
        _ = text ?? throw new ArgumentNullException(nameof(text), "Text mustn't be null.");

        var dbText = _textsMapper.Map(text);
        dbText.Id = Guid.Empty;

        await _textsDao.CreateTextAsync(dbText);

        text.Id = dbText.Id;
    }
}