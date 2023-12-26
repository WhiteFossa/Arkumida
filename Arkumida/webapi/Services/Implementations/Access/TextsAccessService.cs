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

using webapi.Dao.Abstract;
using webapi.Services.Abstract.Access;

namespace webapi.Services.Implementations.Access;

public class TextsAccessService : ITextsAccessService
{
    private readonly ITextsDao _textsDao;

    public TextsAccessService
    (
        ITextsDao textsDao
    )
    {
        _textsDao = textsDao;
    }
    
    public async Task<bool> IsCreatureRelatedToTextAsync(Guid textId, Guid creatureId)
    {
        var textMetadata = await _textsDao.GetTextMetadataByIdAsync(textId);

        if (creatureId == textMetadata.Publisher.Id)
        {
            return true;
        }

        if (textMetadata.Authors.Select(a => a.Id).Contains(creatureId))
        {
            return true;
        }

        if (textMetadata.Translators.Select(t => t.Id).Contains(creatureId))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsVotesHistoryVisibleAsync(Guid textId, Guid? creatureId)
    {
        // Uploader, translators and authors will see history, others - not
        if (!creatureId.HasValue)
        {
            return false;
        }

        return await IsCreatureRelatedToTextAsync(textId, creatureId.Value);
    }
}