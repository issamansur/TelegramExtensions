using System.Text.Json.Serialization;

namespace TLibrary.Models.WebApp;

public class WebAppUser
{
    [JsonPropertyName("id")]
    public long Id { get; init; }
    [JsonPropertyName("first_name")]
    public string FirstName { get; init; }
    [JsonPropertyName("last_name")]
    public string? LastName { get; init; }
    [JsonPropertyName("username")]
    public string? Username { get; init; }
    [JsonPropertyName("photo_url")]
    public string? PhotoUrl { get; init; }
    [JsonPropertyName("language_code")]
    public string? LanguageCode { get; init; }
    [JsonPropertyName("is_bot")]
    public bool? IsBot { get; init; }
    [JsonPropertyName("is_premium")]
    public bool? IsPremium { get; init; }
    [JsonPropertyName("added_to_attachment_menu")]
    public bool? AddedToAttachmentMenu { get; init; }
}
