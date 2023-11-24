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

using System.Net.Http.Headers;
using System.Text.Json;
using furtails_importer.Importers;
using furtails_importer.WebClientStuff.Responses;

namespace furtails_importer.Helpers;

/// <summary>
/// Stuff to work with files
/// </summary>
public static class FilesHelper
{
    public static async Task<UploadFileResponse> UploadFileToArkumidaAsync(HttpClient client, string filename, string mimeType, byte[] content)
    {
        var streamContent = new StreamContent(new MemoryStream(content));
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{MainImporter.BaseUrl}Files/Upload");
        using var requestContent = new MultipartFormDataContent
        {
            {
                streamContent,
                "file",
                filename
            }
        };
        
        request.Content = requestContent;
        
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<UploadFileResponse>(await response.Content.ReadAsStringAsync());
    }
}