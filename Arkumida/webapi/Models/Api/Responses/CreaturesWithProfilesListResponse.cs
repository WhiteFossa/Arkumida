using System.Text.Json.Serialization;
using webapi.Models.Api.DTOs;

namespace webapi.Models.Api.Responses;

/// <summary>
/// Response for methods, listing creatures (with profiles)
/// </summary>
public class CreaturesWithProfilesListResponse
{
    /// <summary>
    /// Listed creatures
    /// </summary>
    [JsonPropertyName("creatures")]
    public IReadOnlyCollection<CreatureWithProfileDto> Creatures { get; private set; }

    public CreaturesWithProfilesListResponse
    (
        IReadOnlyCollection<CreatureWithProfileDto> creatures
    )
    {
        Creatures = creatures ?? throw new ArgumentNullException(nameof(creatures), "Creatures list mustn't be null!");
    }
}