using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Class representing an HTTP response
    /// in Nightinagle.
    /// </summary>
    public class WorkspaceResponse
    {
        /// <summary>
        /// Flag if the response was successful.
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Raw request URL used that led to this response.
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Response HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response status description
        /// extracted from the HTTP response.
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Time in ms for the request's duration.
        /// </summary>
        public long TimeElapsed { get; set; }

        /// <summary>
        /// Body response.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Response content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// List of raw cookies.
        /// </summary>
        public List<KeyValuePair<string, string>> Cookies { get; set; }

        /// <summary>
        /// List of raw headers.
        /// </summary>
        public List<KeyValuePair<string, string>> Headers { get; set; }

        /// <summary>
        /// Log information for the response.
        /// </summary>
        public string Log { get; set; }
    }
}