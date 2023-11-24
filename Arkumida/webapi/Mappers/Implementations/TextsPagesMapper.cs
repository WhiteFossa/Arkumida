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

public class TextsPagesMapper : ITextsPagesMapper
{
    private readonly ITextsSectionsMapper _sectionsMapper;

    public TextsPagesMapper(ITextsSectionsMapper sectionsMapper)
    {
        _sectionsMapper = sectionsMapper;
    }
    
    public IReadOnlyCollection<TextPage> Map(IEnumerable<TextPageDbo> pages)
    {
        if (pages == null)
        {
            return null;
        }

        return pages.Select(p => Map(p)).ToList();
    }

    public TextPage Map(TextPageDbo page)
    {
        if (page == null)
        {
            return null;
        }

        return new TextPage()
        {
            Id = page.Id,
            Number = page.Number,
            Sections = _sectionsMapper.Map(page.Sections)?.ToList()
        };
    }

    public TextPageDbo Map(TextPage page)
    {
        if (page == null)
        {
            return null;
        }

        return new TextPageDbo()
        {
            Id = page.Id,
            Number = page.Number,
            Sections = _sectionsMapper.Map(page.Sections)?.ToList()
        };
    }

    public IReadOnlyCollection<TextPageDbo> Map(IEnumerable<TextPage> pages)
    {
        if (pages == null)
        {
            return null;
        }

        return pages.Select(p => Map(p)).ToList();
    }
}