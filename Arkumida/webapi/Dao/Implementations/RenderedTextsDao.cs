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

using Microsoft.EntityFrameworkCore;
using webapi.Dao.Abstract;
using webapi.Dao.Models;
using webapi.Dao.Models.Enums.RenderedTexts;

namespace webapi.Dao.Implementations;

public class RenderedTextsDao : IRenderedTextsDao
{
    private readonly MainDbContext _dbContext;

    public RenderedTextsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateRenderedTextAsync(RenderedTextDbo renderedText)
    {
        _ = renderedText ?? throw new ArgumentNullException(nameof(renderedText), "Rendered text must not be null");

        // Loading text
        renderedText.Text = _dbContext.Texts.Single(t => t.Id == renderedText.Text.Id);
        
        // Loading file
        renderedText.File = _dbContext.Files.Single(f => f.Id == renderedText.File.Id);
        
        await _dbContext
            .RenderedTexts
            .AddAsync(renderedText);
        
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<RenderedTextDbo> GetRenderedTextAsync(Guid textId, RenderedTextType type)
    {
        return await _dbContext
            .RenderedTexts
            .Include(rt => rt.Text)
            .Include(rt => rt.File)
            .SingleOrDefaultAsync(rt => rt.Text.Id == textId && rt.Type == type);
    }
}