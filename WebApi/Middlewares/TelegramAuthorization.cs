using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using TLibrary.Models.WebApp;
using TLibrary.Utils;

namespace WebApi.Middlewares;

public class TelegramOptions : AuthenticationSchemeOptions
{
    public string BotToken { get; set; }
    public int ExpirationTime { get; set; }
    // Add more options if needed
}

public class TelegramTokenHandler : AuthenticationHandler<TelegramOptions>
{
    private readonly ILogger<TelegramTokenHandler> _logger;

    public TelegramTokenHandler(
        IOptionsMonitor<TelegramOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        _logger = logger.CreateLogger<TelegramTokenHandler>();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Parse token from header 
        // (it should be in format "Telegram <WebAppInitData>")
        var token = Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(token) || !token.StartsWith("Telegram "))
        {
            return Task.FromResult(AuthenticateResult.Fail("Token not provided"));
        }

        var webAppInitDataString = token.Replace("Telegram ", "");

        // Validate token and get WebAppInitData
        WebAppInitData? webAppInitData = null;
        try
        {
            webAppInitData = Validator.ValidateWebAppInitData(webAppInitDataString, Options.BotToken);
        }
        catch (ArgumentException e)
        {
            return Task.FromResult(AuthenticateResult.Fail(e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while validating token");
            return Task.FromResult(AuthenticateResult.Fail("Wow, create a bug report!"));
        }

        if (webAppInitData is not null)
        {
            // Validate token expiration
            Console.WriteLine(DateTimeOffset.Now.ToUnixTimeSeconds());
            Console.WriteLine(webAppInitData.AuthDate);
            if (DateTimeOffset.Now.ToUnixTimeSeconds() - webAppInitData.AuthDate > Options.ExpirationTime)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token expired"));
            }

            // If token is valid, create claims
            var user = webAppInitData.User!;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Locality, user.LanguageCode ?? "en"),
                // Add more claims if needed
            };

            var identity = new ClaimsIdentity(claims, nameof(TelegramTokenHandler));
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        else
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }
    }
}

