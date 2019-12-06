using Nightingale.Core.Models;

namespace Nightingale.Core.Auth
{
    public interface IOauth2Data
    {
        string OAuth2AccessTokenUrl { get; set; }
        string OAuth2AuthUrl { get; set; }
        string OAuth2ClientId { get; set; }
        string OAuth2ClientSecret { get; set; }
        string OAuth2Scope { get; set; }
        string OAuth2AccessToken { get; set; }
        string OAuth2CallbackUrl { get; set; }
        GrantType OAuth2GrantType { get; set; }
    }
}
