using System.Security.Cryptography;

namespace webapi.Helpers;

/// <summary>
/// Helper to calculate SHA512
/// </summary>
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