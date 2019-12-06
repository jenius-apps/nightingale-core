using Nightingale.Core.Storage;
using Nightingale.Core.Auth;

namespace Nightingale.Core.Models
{
    public enum AuthType
    {
        None,
        Basic,
        OAuth1,
        OAuth2,
        Bearer,
        Digest
    }

    public enum GrantType
    {
        client_credentials,
        authorization_code
    }

    public class Authentication : ModifiableBase, IOauth2Data, IStorageItem, IDeepCloneable
    {
        private AuthType _AuthType;
        private string _BasicUsername;
        private string _BasicPassword;
        private string _OAuth1ConsumerKey;
        private string _OAuth1ConsumerSecret;
        private string _OAuth1AccessToken;
        private string _OAuth1TokenSecret;
        private string _OAuth2AccessTokenUrl;
        private string _OAuth2AuthUrl;
        private string _OAuth2ClientId;
        private string _OAuth2ClientSecret;
        private string _OAuth2Scope;
        private string _OAuth2AccessToken;
        private string _OAuth2CallbackUrl;
        private GrantType _OAuth2GrantType;
        private string _BearerToken;
        private string _DigestUsername;
        private string _DigestPassword;

        public Authentication()
        {
        }

        public Authentication(bool isNew)
        {
            if (isNew)
            {
                Status = ModifiedStatus.New;
            }
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public AuthType AuthType
        {
            get => _AuthType;
            set
            {
                if (_AuthType != value)
                {
                    _AuthType = value;
                    ObjectModified();
                }
            }
        }

        // Basic
        public string BasicUsername { get => _BasicUsername; set { ObjectModified(_BasicUsername, value); _BasicUsername = value; } }
        public string BasicPassword { get => _BasicPassword; set { ObjectModified(_BasicPassword, value); _BasicPassword = value; } }

        // OAuth1
        public string OAuth1ConsumerKey { get => _OAuth1ConsumerKey; set { ObjectModified(_OAuth1ConsumerKey, value); _OAuth1ConsumerKey = value;  } }
        public string OAuth1ConsumerSecret { get => _OAuth1ConsumerSecret; set { ObjectModified(_OAuth1ConsumerSecret, value); _OAuth1ConsumerSecret = value;  } }
        public string OAuth1AccessToken { get => _OAuth1AccessToken; set { ObjectModified(_OAuth1AccessToken, value); _OAuth1AccessToken = value; } }
        public string OAuth1TokenSecret { get => _OAuth1TokenSecret; set { ObjectModified(_OAuth1TokenSecret, value); _OAuth1TokenSecret = value; } }

        // OAuth2
        public string OAuth2AccessTokenUrl { get => _OAuth2AccessTokenUrl; set { ObjectModified(_OAuth2AccessTokenUrl, value); _OAuth2AccessTokenUrl = value; } }
        public string OAuth2AuthUrl { get => _OAuth2AuthUrl; set { ObjectModified(_OAuth2AuthUrl, value); _OAuth2AuthUrl = value; } }
        public string OAuth2ClientId { get => _OAuth2ClientId; set { ObjectModified(_OAuth2ClientId, value); _OAuth2ClientId = value; } }
        public string OAuth2ClientSecret { get => _OAuth2ClientSecret; set { ObjectModified(_OAuth2ClientSecret, value); _OAuth2ClientSecret = value; } }
        public string OAuth2Scope { get => _OAuth2Scope; set { ObjectModified(_OAuth2Scope, value); _OAuth2Scope = value; } }
        public string OAuth2AccessToken { get => _OAuth2AccessToken; set { ObjectModified(_OAuth2AccessToken, value); _OAuth2AccessToken = value; } }
        public string OAuth2CallbackUrl { get => _OAuth2CallbackUrl; set { ObjectModified(_OAuth2CallbackUrl, value); _OAuth2CallbackUrl = value; } }

        public GrantType OAuth2GrantType
        {
            get => _OAuth2GrantType;
            set
            {
                if (_OAuth2GrantType != value)
                {
                    _OAuth2GrantType = value;
                    ObjectModified();
                }
            }
        }

        // Bearer
        public string BearerToken { get => _BearerToken; set { ObjectModified(_BearerToken, value); _BearerToken = value; } }

        // Digest
        public string DigestUsername { get => _DigestUsername; set { ObjectModified(_DigestUsername, value); _DigestUsername = value; } }
        public string DigestPassword { get => _DigestPassword; set { ObjectModified(_DigestPassword, value); _DigestPassword = value; } }

        public object DeepClone()
        {
            var result = new Authentication
            {
                AuthType = this.AuthType,

                BasicUsername = this.BasicUsername,
                BasicPassword = this.BasicPassword,

                OAuth1ConsumerKey = this.OAuth1ConsumerKey,
                OAuth1ConsumerSecret = this.OAuth1ConsumerSecret,
                OAuth1AccessToken = this.OAuth1AccessToken,
                OAuth1TokenSecret = this.OAuth1TokenSecret,

                OAuth2AccessTokenUrl = this.OAuth2AccessTokenUrl,
                OAuth2ClientId = this.OAuth2ClientId,
                OAuth2ClientSecret = this.OAuth2ClientSecret,
                OAuth2Scope = this.OAuth2Scope,
                OAuth2AccessToken = this.OAuth2AccessToken,
                OAuth2GrantType = this.OAuth2GrantType,
                OAuth2AuthUrl = this.OAuth2AuthUrl,
                OAuth2CallbackUrl = this.OAuth2CallbackUrl,

                BearerToken = this.BearerToken,

                DigestUsername = this.DigestUsername,
                DigestPassword = this.DigestPassword,

                Status = ModifiedStatus.New
            };

            return result;
        }
    }
}
