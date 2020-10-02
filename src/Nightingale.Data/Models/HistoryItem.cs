using System;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Object designed to be a historical snapshot of a
    /// request item that was sent.
    /// </summary>
    public class HistoryItem : Item
    {
        /// <summary>
        /// Last used date of this history item.
        /// </summary>
        public DateTime LastUsedDate { get; set; }
    }
}