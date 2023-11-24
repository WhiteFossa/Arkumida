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

namespace webapi.Helpers;

/// <summary>
/// Helper to work with files
/// </summary>
public class FilesHelper
{
    /// <summary>
    /// Replace invalid characters in filename with this string
    /// </summary>
    private const string InvalidFilenameCharacterSubstitution = "_";
    
    /// <summary>
    /// Gets user-provided filename and replaces invalid characters with FilesHelper.InvalidFilenameCharacterSubstitution
    /// </summary>
    /// <param name="originalFilename">Filename, which may contain invalid characters</param>
    /// <returns>Filename, where invalid characters are escaped</returns>
    public static string EscapeFilename(string originalFilename)
    {
        return string.Join(InvalidFilenameCharacterSubstitution, originalFilename.Split(Path.GetInvalidFileNameChars())); 
    }
}