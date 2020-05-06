using Nightingale.Core.Auth.Enums;
using Nightingale.Core.Common;
using System.Collections.Generic;

namespace Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Class representing an the authentication
    /// models used by a request.
    /// </summary>
    public class Authentication : IDeepCloneable<Authentication>
    {
        /// <summary>
        /// The current type of authentication.
        /// </summary>
        public AuthType AuthType { get; set; }

        /// <summary>
        /// Properties for auth such as
        /// OAuth access tokens, or bearer tokens, etc.
        /// Usually used with AuthConstants and AuthExtensions.
        /// </summary>
        /// <remarks>
        /// This design was chosen because there are many
        /// string values required for all the authentication methods.
        /// So it was not scalable, nor was it readable, to define
        /// every string value required for each authentication type.
        /// </remarks>
        public Dictionary<string, string> AuthProperties { get; set; }

        /// <inheritdoc/>
        public Authentication DeepClone()
        {
            var other = new Authentication
            {
                AuthType = this.AuthType,
                AuthProperties = this.AuthProperties != null
                    ? new Dictionary<string, string>(this.AuthProperties)
                    : new Dictionary<string, string>(),
            };

            return other;
        }
    }
}
