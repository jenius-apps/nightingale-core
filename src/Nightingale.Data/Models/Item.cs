using Newtonsoft.Json;
using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// This workspace item represents an object that
    /// can be displayed in the tree of the workspace.
    /// </summary>
    public class Item : IHasParent
    {
        /// <summary>
        /// GUID for this item.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc/>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// If true, the item is just a tab
        /// and it hasn't been saved to the workspace item tree.
        /// </summary>
        public bool IsTemporary { get; set; }

        /// <summary>
        /// A dictionary of properties usually
        /// used by a GUI app.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Url of item.
        /// </summary>
        public Url Url { get; set; }

        /// <summary>
        /// Authentication configuration for item.
        /// </summary>
        public Authentication Auth { get; set; }

        /// <summary>
        /// Request body configuration for item.
        /// </summary>
        public RequestBody Body { get; set; }

        /// <summary>
        /// Data used for API mocking.
        /// </summary>
        public MockData MockData { get; set; }

        /// <summary>
        /// List of children for this item.
        /// </summary>
        public List<Item> Children { get; set; }

        /// <summary>
        /// List of headers.
        /// </summary>
        public List<Parameter> Headers { get; set; }

        /// <summary>
        /// List of chaining rules to be used after
        /// a request is sent.
        /// </summary>
        public List<Parameter> ChainingRules { get; set; }

        /// <summary>
        /// The item's type.
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// Name of this item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag for whether this item is displaying
        /// its children in the UI or not.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// HTTP method to be used for this request.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Response of request.
        /// </summary>
        public WorkspaceResponse Response { get; set; }
    }

    /// <summary>
    /// Enum for the type of item
    /// this should be displayed in the tree.
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// No type. Shouldn't be used
        /// in normal scenarios. Only added
        /// as a neutral default.
        /// </summary>
        None,

        /// <summary>
        /// Item is a request object.
        /// </summary>
        Request,

        /// <summary>
        /// Item is a collection of requests.
        /// </summary>
        Collection
    }
}