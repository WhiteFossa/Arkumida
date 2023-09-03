using System.Text.Json.Serialization;

namespace webapi.Models.Api.Requests;

/// <summary>
/// Request to rename creature
/// </summary>
public class RenameCreatureRequest
{
    /// <summary>
    /// New name
    /// </summary>
    [JsonPropertyName("newName")]
    public string NewName { get; set; }
}