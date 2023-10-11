using Microsoft.Extensions.Options;
using OpenSearch.Client;
using OpenSearch.Net;
using webapi.Models.Settings;
using webapi.OpenSearch.Models;
using webapi.OpenSearch.Services.Abstract;

namespace webapi.OpenSearch.Services.Implementations;

public class ArkumidaOpenSearchClient : IArkumidaOpenSearchClient
{
    private readonly OpenSearchSettings _openSearchSettings;

    private OpenSearchClient _client;
    
    public ArkumidaOpenSearchClient
    (
        IOptions<OpenSearchSettings> openSearchSettings
    )
    {
        _openSearchSettings = openSearchSettings.Value;
        
        Connect();
    }
    
    public void Connect()
    {
        if (string.IsNullOrEmpty(_openSearchSettings.Url))
        {
            throw new InvalidOperationException("OpenSearch connection is not configured. Configure it in appsettings.json, please.");
        }

        if (_client != null)
        {
            return;
        }
        
        _client = new OpenSearchClient(new Uri(_openSearchSettings.Url));
    }

    public async Task<string> IndexCreatureAsync(IndexableCreature creatureToIndex)
    {
        if (creatureToIndex == null)
        {
            throw new ArgumentNullException(nameof(creatureToIndex), "Attempt to index a null creature!");
        }
        
        var response = await _client
            .IndexAsync(creatureToIndex, i => i.Index(creatureToIndex.GetIndexName())
            .Refresh(Refresh.WaitFor));

        if (!response.IsValid)
        {
            throw new InvalidOperationException($"Can't index a creature! Debug information: { response.DebugInformation }");
        }

        return response.Id;
    }
}