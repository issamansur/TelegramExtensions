using System.Text.Json;
using System.Web;
using TLibrary.Models.WebApp;

namespace TLibrary.Utils;

public static class Converters
{
    public static WebAppInitData? GetWebAppInitData(this string initDataString)
    {
        var data = HttpUtility.ParseQueryString(initDataString);

        var authDate = data["auth_date"]!;
        var hash = data["hash"]!;
        var queryId = data["query_id"];
        var user = data["user"];
        var receiver = data["receiver"];
        var chat = data["chat"];
        var chatType = data["chat_type"];
        var chatInstance = data["chat_instance"];
        var startParam = data["start_param"];
        var canSendAfter = data["can_send_after"];

        return new WebAppInitData
        {
            AuthDate = long.Parse(authDate),
            Hash = hash,
            QueryId = queryId,
            User = user?.GetWebAppUser(),
            Receiver = receiver?.GetWebAppUser(),
            Chat = chat?.GetWebAppChat(),
            ChatType = chatType,
            ChatInstance = chatInstance,
            StartParam = startParam,
            CanSendAfter = canSendAfter == null ? null : int.Parse(canSendAfter)
        };
    }

    public static WebAppUser? GetWebAppUser(this string userJson) =>
        JsonSerializer.Deserialize<WebAppUser>(userJson);

    public static WebAppChat? GetWebAppChat(this string chatJson) =>
        JsonSerializer.Deserialize<WebAppChat>(chatJson);
}