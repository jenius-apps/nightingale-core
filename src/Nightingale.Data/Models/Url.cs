using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// An object representing
    /// a URL string.
    /// </summary>
    public class Url
    {
        /// <summary>
        /// String representation of the URL without queries.
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// The queries in the URL string.
        /// </summary>
        public List<Parameter> Queries { get; set; }
    }
}