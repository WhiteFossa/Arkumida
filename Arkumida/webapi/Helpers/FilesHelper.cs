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
        return string.Join(FilesHelper.InvalidFilenameCharacterSubstitution, originalFilename.Split(Path.GetInvalidFileNameChars())); 
    }
}