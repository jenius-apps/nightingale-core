using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// A Nightingale workspace.
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// GUID for this workspace.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of workspace.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of HTTP methods enabled
        /// for this workspace.
        /// </summary>
        public List<string> Methods { get; set; }

        /// <summary>
        /// List of items that were open as a tab at the
        /// time of closing the workspace.
        /// </summary>
        /// <remarks>
        /// The intention of this property is
        /// to provide a way to restore the open tabs
        /// when the customer re-opens Nightingale.
        /// </remarks>
        public List<string> OpenItemIds { get; set; }

        /// <summary>
        /// List of items that are shown as tabs
        /// but are not stored in the Items tree.
        /// </summary>
        public List<Item> TempItems { get; set; }

        /// <summary>
        /// Tree of items in this workspace.
        /// </summary>
        public List<Item> Items { get; set; }

        /// <summary>
        /// List of history items for this workspace.
        /// </summary>
        public List<HistoryItem> HistoryItems { get; set; }

        /// <summary>
        /// List of environments for this workspace.
        /// </summary>
        public List<Envronment> Environments { get; set; }

        /// <summary>
        /// List of HTTP cookies for this workspace.
        /// </summary>
        public List<Cookie> Cookies { get; set; }
    }
}