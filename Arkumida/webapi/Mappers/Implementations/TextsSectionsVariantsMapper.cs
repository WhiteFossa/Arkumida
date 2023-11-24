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

public class TextsSectionsVariantsMapper : ITextsSectionsVariantsMapper
{
    public IReadOnlyCollection<TextSectionVariant> Map(IEnumerable<TextSectionVariantDbo> variants)
    {
        if (variants == null)
        {
            return null;
        }

        return variants.Select(v => Map(v)).ToList();
    }

    public TextSectionVariant Map(TextSectionVariantDbo variant)
    {
        if (variant == null)
        {
            return null;
        }

        return new TextSectionVariant()
        {
            Id = variant.Id,
            Content = variant.Content,
            CreationTime = variant.CreationTime
        };
    }

    public TextSectionVariantDbo Map(TextSectionVariant variant)
    {
        if (variant == null)
        {
            return null;
        }

        return new TextSectionVariantDbo()
        {
            Id = variant.Id,
            Content = variant.Content,
            CreationTime = variant.CreationTime
        };
    }

    public IReadOnlyCollection<TextSectionVariantDbo> Map(IEnumerable<TextSectionVariant> variants)
    {
        if (variants == null)
        {
            return null;
        }

        return variants.Select(v => Map(v)).ToList();
    }
}