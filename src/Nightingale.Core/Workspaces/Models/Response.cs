using JeniusApps.Nightingale.Core.Common;
using JeniusApps.Nightingale.Core.Workspaces.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Represents an http response.
    /// </summary>
    public class Response : ObservableBase, IDeepCloneable<Response>
    {
        /// <summary>
        /// Success boolean for response.
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// The original request URL.
        /// </summary>
        public string RequestBaseUrl { get; set; }

        /// <summary>
        /// Http status code of response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Http response description.
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// The time it took for the request to complete.
        /// </summary>
        public long TimeElapsed { get; set; }

        /// <summary>
        /// The content of the response as raw bytes.
        /// </summary>
        public byte[] RawBytes { get; set; }

        /// <summary>
        /// The content of the response as string.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The response's content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// List of cookies for this response.
        /// </summary>
        public IList<KeyValuePair<string, string>> Cookies { get; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// List of headers for this reponses.
        /// </summary>
        public IList<KeyValuePair<string, string>> Headers { get; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Log output for this response.
        /// </summary>
        public string Log { get; set; }

        /// <inheritdoc/>
        public Response DeepClone()
        {
            var other = new Response
            {
                Log = this.Log,
                ContentType = this.ContentType,
                Body = this.Body,
                RawBytes = this.RawBytes.ToArray(),
                StatusDescription = this.StatusDescription,
                RequestBaseUrl = this.RequestBaseUrl,
                StatusCode = this.StatusCode,
                Successful = this.Successful,
                TimeElapsed = this.TimeElapsed
            };
            other.Cookies.DeepCloneStructs(this.Cookies);
            other.Headers.DeepCloneStructs(this.Headers);
            return other;
        }
    }
}
