#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
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

using System.Security.Cryptography;

namespace furtails_importer.Helpers;

public class SHA512Helper
{
    /// <summary>
    /// Calculate SHA512 hash of data stream
    /// </summary>
    public static string CalculateSHA512(Stream data)
    {
        data.Seek(0, SeekOrigin.Begin);

        var calculator = SHA512.Create();
        var resultBytes = calculator.ComputeHash(data);

        return Convert.ToBase64String(resultBytes, Base64FormattingOptions.None);
    }
    
    /// <summary>
    /// Calculate SHA512 hash of file contents
    /// </summary>
    public static string CalculateSHA512(byte[] data)
    {
        var calculator = SHA512.Create();
        var resultBytes = calculator.ComputeHash(data);

        return Convert.ToBase64String(resultBytes, Base64FormattingOptions.None);
    }
}