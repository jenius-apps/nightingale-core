using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// The authentication configuration
    /// for an item.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// This item's type.
        /// </summary>
        public AuthType AuthType { get; set; }

        /// <summary>
        /// Dictionary of various auth configurations.
        /// </summary>
        public Dictionary<string, string> AuthProperties { get; set; }
    }

    /// <summary>
    /// The authentication type.
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// No authentication to use.
        /// </summary>
        None,

        /// <summary>
        /// Use basic HTTP authentication.
        /// </summary>
        Basic,

        /// <summary>
        /// Use OAuth 1.0a authentication.
        /// </summary>
        OAuth1,

        /// <summary>
        /// Use OAuth 2.0 authentication.
        /// </summary>
        OAuth2,

        /// <summary>
        /// Use bearer token authentication.
        /// </summary>
        Bearer,

        /// <summary>
        /// Use digest authentication.
        /// </summary>
        Digest,

        /// <summary>
        /// Inherit the item's parent's authenticaiton configuration.
        /// </summary>
        InheritParent
    }

    /// <summary>
    /// The grant type for use with
    /// OAuth 2.0.
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// Use client credential grant type for OAuth 2.0.
        /// </summary>
        client_credentials,

        /// <summary>
        /// Use auth code grant type for OAuth 2.0.
        /// </summary>
        authorization_code,

        /// <summary>
        /// Use implicit flow grant type for OAuth 2.0.
        /// </summary>
        implicit_flow
    }
}