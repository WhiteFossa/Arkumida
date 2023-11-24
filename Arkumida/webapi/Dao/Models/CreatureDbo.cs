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

using Microsoft.AspNetCore.Identity;

namespace webapi.Dao.Models;

/// <summary>
/// Arkumida user (users, authors, translators etc)
/// </summary>
public class CreatureDbo : IdentityUser<Guid>
{
    /// <summary>
    /// This creature is author of the next texts
    /// </summary>
    public IList<TextDbo> TextsAuthor { get; set; }
    
    /// <summary>
    /// This creature is translator of the next texts
    /// </summary>
    public IList<TextDbo> TextsTranslator { get; set; }
    
    /// <summary>
    /// The creature is author for the next private messages
    /// </summary>
    public IList<PrivateMessageDbo> SenderOfThisPrivateMessages { get; set; }
    
    /// <summary>
    /// The creature is receiver of the next private messages
    /// </summary>
    public IList<PrivateMessageDbo> ReceiverOfThisPrivateMessages { get; set; }
}