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

using System.ComponentModel.DataAnnotations;

namespace webapi.Dao.Models;

/// <summary>
/// Database model for private message
/// </summary>
public class PrivateMessageDbo
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Message sender
    /// </summary>
    public CreatureDbo Sender { get; set; }
    
    /// <summary>
    /// Message receiver
    /// </summary>
    public CreatureDbo Receiver { get; set; }

    /// <summary>
    /// Message was sent at this time
    /// </summary>
    public DateTime SentTime { get; set; }
    
    /// <summary>
    /// Message was read (if was) at this time
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// Is message received on receiver side
    /// </summary>
    public bool IsDeletedOnReceiverSide { get; set; }
    
    /// <summary>
    /// Is message received on receiver side
    /// </summary>
    public bool IsDeletedOnSenderSide { get; set; }
}