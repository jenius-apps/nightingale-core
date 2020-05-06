using Nightingale.Core.Common;
using System.Linq;

namespace Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// A class representing an HTTP cookie.
    /// </summary>
    public class Cookie : ObservableBase, IDeepCloneable<Cookie>
    {
        /// <summary>
        /// Gets or sets the cookie's domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the cookie's raw string.
        /// </summary>
        public string Raw { get; set; }

        /// <summary>
        /// Parses the raw cookie string
        /// to return the cookie's name.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Raw) || !Raw.Contains("="))
                {
                    return "";
                }

                var split = Raw.Split('=');
                return split.FirstOrDefault();
            }
        }

        /// <summary>
        /// Parses the raw cookie string to return
        /// the cookie's value.
        /// </summary>
        public string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Raw))
                {
                    return "";
                }

                var split = Raw.Split(';');

                foreach (var segment in split)
                {
                    if (segment.Contains("="))
                    {
                        var segmentSplit = segment.Split('=');
                        return segmentSplit.LastOrDefault();
                    }
                }

                return "";
            }
        }

        /// <inheritdoc/>
        public Cookie DeepClone()
        {
            return new Cookie
            {
                Raw = this.Raw,
                Domain = this.Domain,
            };
        }
    }
}
