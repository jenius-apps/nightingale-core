using System;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Class representing a URL recently
    /// used by the user.
    /// </summary>
    public class RecentUrl
    {
        /// <summary>
        /// The URL string.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Time when used.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Id of parent.
        /// </summary>
        public string ParentId { get; set; }    
    }
}