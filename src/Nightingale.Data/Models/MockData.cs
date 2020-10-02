namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Data used for a mock server.
    /// </summary>
    public class MockData
    {
        /// <summary>
        /// Body to return when using a mock server.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Status to return when using a mock server.
        /// Default 200.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Content type to return when using a mock server.
        /// </summary>
        public string ContentType { get; set; }
    }
}