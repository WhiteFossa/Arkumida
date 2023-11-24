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

namespace webapi.Models.Email;

/// <summary>
/// One email message
/// </summary>
public class Email
{
    // Receiver
    public List<string> To { get; private set; }
    public List<string> Bcc { get; private set; }

    public List<string> Cc { get; private set; }

    // Sender
    public string From { get; private set; }

    public string DisplayName { get; private set; }

    public string ReplyTo { get; private set; }

    public string ReplyToName { get; private set; }

    // Content
    public string Subject { get; private set; }

    public string Body { get; private set; }

    public Email
    (
        List<string> to,
        string subject,
        string body,
        string from = null,
        string displayName = null,
        string replyTo = null,
        string replyToName = null,
        List<string> bcc = null,
        List<string> cc = null
    )
    {
        // Receiver
        To = to;
        Bcc = bcc ?? new List<string>();
        Cc = cc ?? new List<string>();

        // Sender
        From = from;
        DisplayName = displayName;
        ReplyTo = replyTo;
        ReplyToName = replyToName;
            
        // Content
        Subject = subject;
        Body = body;
    }
}