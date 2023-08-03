using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextsMapper : ITextsMapper
{
    private readonly ITextsPagesMapper _pagesMapper;
    private readonly ITagsMapper _tagsMapper;
    private readonly ITextFilesMapper _textFilesMapper;
    private readonly ICreaturesMapper _creaturesMapper;

    public TextsMapper
    (
        ITextsPagesMapper pagesMapper,
        ITagsMapper tagsMapper,
        ITextFilesMapper textFilesMapper,
        ICreaturesMapper creaturesMapper
    )
    {
        _pagesMapper = pagesMapper;
        _tagsMapper = tagsMapper;
        _textFilesMapper = textFilesMapper;
        _creaturesMapper = creaturesMapper;
    }
    
    public IReadOnlyCollection<Text> Map(IEnumerable<TextDbo> texts)
    {
        if (texts == null)
        {
            return null;
        }

        return texts.Select(t => Map(t)).ToList();
    }

    public Text Map(TextDbo text)
    {
        if (text == null)
        {
            return null;
        }
        
        return new Text()
        {
            Id = text.Id,
            CreateTime = text.CreateTime,
            LastUpdateTime = text.LastUpdateTime,
            Title = text.Title,
            Description = text.Description,
            Pages = _pagesMapper.Map(text.Pages).ToList(),
            ReadsCount = text.ReadsCount,
            VotesCount = text.VotesCount,
            VotesPlus = text.VotesPlus,
            VotesMinus = text.VotesMinus,
            Tags = _tagsMapper.Map(text.Tags).ToList(),
            IsIncomplete = text.IsIncomplete,
            TextFiles = _textFilesMapper.Map(text.TextFiles).ToList(),
            Author = _creaturesMapper.Map(text.Author),
            Publisher = _creaturesMapper.Map(text.Publisher),
            Translator = _creaturesMapper.Map(text.Translator)
        };
    }

    public TextDbo Map(Text text)
    {
        if (text == null)
        {
            return null;
        }

        return new TextDbo()
        {
            Id = text.Id,
            CreateTime = text.CreateTime,
            LastUpdateTime = text.LastUpdateTime,
            Title = text.Title,
            Description = text.Description,
            Pages = _pagesMapper.Map(text.Pages).ToList(),
            ReadsCount = text.ReadsCount,
            VotesCount = text.VotesCount,
            VotesPlus = text.VotesPlus,
            VotesMinus = text.VotesMinus,
            Tags = _tagsMapper.Map(text.Tags).ToList(),
            IsIncomplete = text.IsIncomplete,
            TextFiles = _textFilesMapper.Map(text.TextFiles).ToList(),
            Author = _creaturesMapper.Map(text.Author),
            Publisher = _creaturesMapper.Map(text.Publisher),
            Translator = _creaturesMapper.Map(text.Translator)
        };
    }

    public IReadOnlyCollection<TextDbo> Map(IEnumerable<Text> texts)
    {
        if (texts == null)
        {
            return null;
        }

        return texts.Select(t => Map(t)).ToList();
    }
}