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
        None,
        Basic,
        OAuth1,
        OAuth2,
        Bearer,
        Digest,
        InheritParent
    }

    /// <summary>
    /// The grant type for use with
    /// OAuth 2.0.
    /// </summary>
    public enum GrantType
    {
        client_credentials,
        authorization_code,
        implicit_flow
    }
}