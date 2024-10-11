using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TLibrary.Models.WebApp;

public class WebAppChat
{
    [Required]
    [JsonPropertyName("id")]
    public long Id { get; init; }
    [JsonPropertyName("type")]
    public string Type { get; init; }
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; init; }
    [JsonPropertyName("username")]
    public string? Username { get; init; }
    [JsonPropertyName("photo_url")]
    public string? PhotoUrl { get; init; }
}
