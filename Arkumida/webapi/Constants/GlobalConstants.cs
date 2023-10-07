namespace webapi.Constants;

/// <summary>
/// Store global constants here
/// </summary>
public static class GlobalConstants
{
    #region Requests-related

    /// <summary>
    /// Minimum displayNamePart length for UsersController.FindCreaturesByDisplayNamePartAsync() method. If length is less, then empty collection will be returned 
    /// </summary>
    public const int MinFindCreaturesByDisplayNamePartPartLength = 3;

    #endregion
}