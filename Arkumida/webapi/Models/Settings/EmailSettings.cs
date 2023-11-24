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

namespace webapi.Models.Settings;

/// <summary>
/// Appsettings.json email settings section
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Sender address
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Sender name
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// SMTP server address
    /// </summary>
    public string SmtpHost { get; set; }

    /// <summary>
    /// SMTP server port
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// Use SSL for SMTP connection
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Use StartTLS for SMTP connection
    /// </summary>
    public bool UseStartTls { get; set; }

    /// <summary>
    /// SMTP server username
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// SMTP server password
    /// </summary>
    public string Password { get; set; }
}