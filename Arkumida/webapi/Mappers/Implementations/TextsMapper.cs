using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Models;

namespace webapi.Mappers.Implementations;

public class TextsMapper : ITextsMapper
{
    private readonly ITextsSectionsMapper _sectionsMapper;
    private readonly ITagsMapper _tagsMapper;

    public TextsMapper(ITextsSectionsMapper sectionsMapper,
        ITagsMapper tagsMapper)
    {
        _sectionsMapper = sectionsMapper;
        _tagsMapper = tagsMapper;
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
            Sections = _sectionsMapper.Map(text.Sections).ToList(),
            ReadsCount = text.ReadsCount,
            VotesCount = text.VotesCount,
            VotesPlus = text.VotesPlus,
            VotesMinus = text.VotesMinus,
            Tags = _tagsMapper.Map(text.Tags).ToList(),
            IsIncomplete = text.IsIncomplete
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
            Sections = _sectionsMapper.Map(text.Sections).ToList(),
            ReadsCount = text.ReadsCount,
            VotesCount = text.VotesCount,
            VotesPlus = text.VotesPlus,
            VotesMinus = text.VotesMinus,
            Tags = _tagsMapper.Map(text.Tags).ToList(),
            IsIncomplete = text.IsIncomplete
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