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