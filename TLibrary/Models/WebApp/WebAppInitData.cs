using System.Text.Json.Serialization;

namespace TLibrary.Models.WebApp;

public class WebAppInitData
{
    [JsonPropertyName("auth_date")]
    public long AuthDate { get; init; }
    [JsonPropertyName("hash")]
    public string Hash { get; init; }
    [JsonPropertyName("query_id")]
    public string? QueryId { get; init; }
    [JsonPropertyName("user")]
    public WebAppUser? User { get; init; }
    [JsonPropertyName("receiver")]
    public WebAppUser? Receiver { get; init; }
    [JsonPropertyName("chat")]
    public WebAppChat? Chat { get; init; }
    [JsonPropertyName("chat_type")]
    public string? ChatType { get; init; }
    [JsonPropertyName("chat_instance")]
    public string? ChatInstance { get; init; }
    [JsonPropertyName("start_param")]
    public string? StartParam { get; init; }
    [JsonPropertyName("can_send_after")]
    public int? CanSendAfter { get; init; }
}
