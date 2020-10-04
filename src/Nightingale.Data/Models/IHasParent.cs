using Newtonsoft.Json;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Interface depicting an item
    /// that contains an ID reference to
    /// its parent.
    /// </summary>
    public interface IHasParent
    {
        /// <summary>
        /// Id of item.
        /// </summary>
        [JsonProperty("id")]
        string Id { get; set; }

        /// <summary>
        /// Id of parent.
        /// </summary>
        [JsonProperty("parentId")]
        string ParentId { get; set; }
    }
}
