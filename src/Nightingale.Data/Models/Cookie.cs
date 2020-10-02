namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// A class representing an HTTP cookie in Nightingale.
    /// </summary>
    public class Cookie
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
        /// GUID for this cookie.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ID of this cookie's parent.
        /// </summary>
        public string ParentId { get; set; }
    }
}