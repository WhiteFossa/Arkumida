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
            Pages = _pagesMapper.Map(text.Pages)?.ToList(),
            Tags = _tagsMapper.Map(text.Tags)?.ToList(),
            IsIncomplete = text.IsIncomplete,
            TextFiles = _textFilesMapper.Map(text.TextFiles)?.ToList(),
            Authors = null, // Must be loaded externally
            Publisher = null, // Must be loaded externally
            Translators = null // Must be loaded externally
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
            Pages = _pagesMapper.Map(text.Pages)?.ToList(),
            Tags = _tagsMapper.Map(text.Tags)?.ToList(),
            IsIncomplete = text.IsIncomplete,
            TextFiles = _textFilesMapper.Map(text.TextFiles)?.ToList(),
            Authors = _creaturesMapper.Map(text.Authors)?.ToList(),
            Publisher = _creaturesMapper.Map(text.Publisher),
            Translators = _creaturesMapper.Map(text.Translators)?.ToList()
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