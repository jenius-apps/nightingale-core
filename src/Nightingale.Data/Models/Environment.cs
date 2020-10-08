using Newtonsoft.Json;
using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Class representing a variable environment
    /// in Nightingale.
    /// </summary>
    public class Environment : IHasParent
    {
        /// <summary>
        /// GUID for this environment.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc/>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// Name of this environment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of environnment.
        /// </summary>
        public EnvType EnvironmentType { get; set; }

        /// <summary>
        /// List of variables for this environment.
        /// </summary>
        public List<Parameter> Variables { get; set; }
    }

    /// <summary>
    /// Type of environment.
    /// </summary>
    public enum EnvType
    {
        /// <summary>
        /// Substitute. Any environment
        /// that isn't the base environment.
        /// </summary>
        Sub,

        /// <summary>
        /// The base environment.
        /// </summary>
        Base
    }
}