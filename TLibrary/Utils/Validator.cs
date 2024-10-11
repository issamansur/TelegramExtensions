using System.Security.Cryptography;
using System.Text;
using System.Web;
using TLibrary.Models.WebApp;

namespace TLibrary.Utils;

public static class Validator
{
    private const string tgKey = "WebAppData";

    /// <summary>
    /// Validate webAppInitData and return WebAppInitData object or null.
    /// For more information, visit <see href="https://core.telegram.org/bots/webapps#validating-data-received-via-the-mini-app">our website</see>.
    /// </param>
    /// </summary>
    /// <param name="webAppInitData"> Initial data string from telegram </param>
    /// <param name="botToken"> Bot token </param>
    /// <returns> WebAppInitData object if data is valid, otherwise null </returns>
    public static WebAppInitData? ValidateWebAppInitData(string webAppInitData, string botToken)
    {
        if (Validate(webAppInitData, botToken))
        {
            return webAppInitData.GetWebAppInitData();
        }

        return null;
    }

    /// <summary>
    /// Validate webAppInitData and return result.
    /// </param>
    /// </summary>
    /// <param name="webAppInitData"> Initial data string from telegram </param>
    /// <param name="botToken"> Bot token </param>
    /// <returns> True if data is valid, otherwise false </returns>
    /// <exception cref="ArgumentException"> Thrown when webAppInitData is invalid </exception>
    private static bool Validate(string webAppInitData, string botToken)
    {
        if (string.IsNullOrEmpty(webAppInitData))
        {
            throw new ArgumentException("WebAppInitData is null or empty", nameof(webAppInitData));
        }


        // Get 'dataCheckString' and 'hash' from 'webAppInitData'
        var parameters = HttpUtility.ParseQueryString(webAppInitData, Encoding.UTF8);

        var dataCheckString = string.Join(
            separator: '\n',
            parameters.AllKeys
                .Where(key => key != "hash")
                .OrderBy(key => key)
                .Select(key => $"{key}={parameters[key]}")
        );

        var hash = parameters["hash"];

        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException("WebAppInitData does not contain 'hash' parameter", nameof(webAppInitData));
        }


        // Check hashes with HMAC-SHA256
        var secretKeyBytes = HMACSHA256.HashData(
            source: botToken.ToBytes(),
            key: tgKey.ToBytes()
        );

        var computedHashBytes = HMACSHA256.HashData(
            source: dataCheckString.ToBytes(),
            key: secretKeyBytes
        );


        return computedHashBytes.ToHex() == hash;
    }

    private static byte[] ToBytes(this string source) =>
        Encoding.UTF8.GetBytes(source);

    private static string ToHex(this byte[] source) =>
        Convert.ToHexString(source).ToLower();
}
