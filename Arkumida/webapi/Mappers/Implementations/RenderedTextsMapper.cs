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

public class RenderedTextsMapper : IRenderedTextsMapper
{
    private readonly ITextsMapper _textsMapper;
    private readonly IFilesMapper _filesMapper;

    public RenderedTextsMapper
    (
        ITextsMapper textsMapper,
        IFilesMapper filesMapper
    )
    {
        _textsMapper = textsMapper;
        _filesMapper = filesMapper;
    }
    
    public IReadOnlyCollection<RenderedText> Map(IEnumerable<RenderedTextDbo> renderedTexts)
    {
        if (renderedTexts == null)
        {
            return null;
        }

        return renderedTexts.Select(rt => Map(rt)).ToList();
    }

    public RenderedText Map(RenderedTextDbo renderedText)
    {
        if (renderedText == null)
        {
            return null;
        }

        return new RenderedText()
        {
            Id = renderedText.Id,
            Type = renderedText.Type,
            Text = _textsMapper.Map(renderedText.Text),
            File = _filesMapper.Map(renderedText.File)
        };
    }

    public RenderedTextDbo Map(RenderedText renderedText)
    {
        if (renderedText == null)
        {
            return null;
        }

        return new RenderedTextDbo()
        {
            Id = renderedText.Id,
            Type = renderedText.Type,
            Text = _textsMapper.Map(renderedText.Text),
            File = _filesMapper.Map(renderedText.File)
        };
    }

    public IReadOnlyCollection<RenderedTextDbo> Map(IEnumerable<RenderedText> renderedTexts)
    {
        if (renderedTexts == null)
        {
            return null;
        }

        return renderedTexts.Select(rt => Map(rt)).ToList();
    }
}