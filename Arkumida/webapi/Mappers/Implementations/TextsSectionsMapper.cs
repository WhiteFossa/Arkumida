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

public class TextsSectionsMapper : ITextsSectionsMapper
{
    private readonly ITextsSectionsVariantsMapper _variantsMapper;

    public TextsSectionsMapper(ITextsSectionsVariantsMapper variantsMapper)
    {
        _variantsMapper = variantsMapper;
    }
    
    public IReadOnlyCollection<TextSection> Map(IEnumerable<TextSectionDbo> sections)
    {
        if (sections == null)
        {
            return null;
        }

        return sections.Select(s => Map(s)).ToList();
    }

    public TextSection Map(TextSectionDbo section)
    {
        if (section == null)
        {
            return null;
        }

        return new TextSection()
        {
            Id = section.Id,
            OriginalText = section.OriginalText,
            Order = section.Order,
            Variants = _variantsMapper.Map(section.Variants).ToList()
        };
    }

    public TextSectionDbo Map(TextSection section)
    {
        if (section == null)
        {
            return null;
        }

        return new TextSectionDbo()
        {
            Id = section.Id,
            OriginalText = section.OriginalText,
            Order = section.Order,
            Variants = _variantsMapper.Map(section.Variants).ToList()
        };
    }

    public IReadOnlyCollection<TextSectionDbo> Map(IEnumerable<TextSection> sections)
    {
        if (sections == null)
        {
            return null;
        }

        return sections.Select(s => Map(s)).ToList();
    }
}