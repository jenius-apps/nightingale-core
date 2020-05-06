using Nightingale.Core.Workspaces.Models;

namespace Nightingale.Core.Workspaces.Enums
{
    /// <summary>
    /// The authentication type for an <see cref="Authentication"/>.
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// No authentication.
        /// </summary>
        None,

        /// <summary>
        /// Basic authentication.
        /// </summary>
        Basic,
        
        /// <summary>
        /// OAuth 1.0a authentication.
        /// </summary>
        OAuth1,
        
        /// <summary>
        /// OAuth 2.0 authentication.
        /// </summary>
        OAuth2,

        /// <summary>
        /// Bearer token authentication.
        /// </summary>
        Bearer,

        /// <summary>
        /// Digest authentication.
        /// </summary>
        Digest,

        /// <summary>
        /// The current <see cref="Item"/> will inherit the authentication
        /// of its parent <see cref="Item"/>.
        /// </summary>
        InheritParent
    }
}
