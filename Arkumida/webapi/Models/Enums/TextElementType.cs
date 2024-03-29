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

namespace webapi.Models.Enums;

/// <summary>
/// Possible text element types
/// </summary>
public enum TextElementType
{
    ParagraphBegin = 0,
    
    PlainText = 1,
    
    ParagraphEnd = 2,
    
    FullWidthAlignedTextBegin = 3,
    
    FullWidthAlignedTextEnd = 4,
    
    ItalicBegin = 5,
    
    ItalicEnd = 6,
    
    BoldBegin = 7,
    
    BoldEnd = 8,
    
    UnderlineBegin = 9,
    
    UnderlineEnd = 10,
    
    StrikeOutBegin = 11,
    
    StrikeOutEnd = 12,
    
    CentrallyAlignedTextBegin = 13,
    
    CentrallyAlignedTextEnd = 14,
    
    LeftAlignedTextBegin = 15,
    
    LeftAlignedTextEnd = 16,
    
    RightAlignedTextBegin = 17,
    
    RightAlignedTextEnd = 18,
    
    TitleBegin = 19,
    
    TitleEnd = 20,
    
    PreformattedTextBegin = 21,
    
    PreformattedTextEnd = 22,
    
    QuoteBegin = 23,
    
    QuoteEnd = 24,
    
    AsciiArtBegin = 25,
    
    AsciiArtEnd = 26,
    
    UrlBegin = 27,
    
    UrlEnd = 28,
    
    ColorBegin = 29,
    
    ColorEnd = 30,
    
    SizedAsciiArtBegin = 31,
    
    SizedAsciiArtEnd = 32,
    
    EmbeddedImage = 33,
    
    ComicsImage = 34, // Like embedded image, but preview is full-size,
    
    CreatureQuoteBegin = 35, // Creature's quote (for forum and comments)
    
    CreatureQuoteEnd = 36,
    
    ExternalImage = 37 // External (hotlinked) image
}