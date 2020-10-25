using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Object for a request body.
    /// </summary>
    public class RequestBody
    {
        /// <summary>
        /// This item's type.
        /// </summary>
        public RequestBodyType BodyType { get; set; }

        /// <summary>
        /// JSON body text.
        /// </summary>
        public string JsonBody { get; set; }

        /// <summary>
        /// XML body text.
        /// </summary>
        public string XmlBody { get; set; }

        /// <summary>
        /// Plain text body.
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// List of form encoded data.
        /// </summary>
        public List<Parameter> FormEncodedData { get; set; } = new List<Parameter>();

        /// <summary>
        /// List of form data.
        /// </summary>
        public List<FormData> FormDataList { get; set; } = new List<FormData>();

        /// <summary>
        /// Path for file.
        /// </summary>
        public string BinaryFilePath { get; set; }
    }

    /// <summary>
    /// The type of request body.
    /// </summary>
    public enum RequestBodyType
    {
        /// <summary>
        /// There is no request body.
        /// </summary>
        None,

        /// <summary>
        /// Request body is JSON.
        /// </summary>
        Json,

        /// <summary>
        /// Request body is XML.
        /// </summary>
        Xml,

        /// <summary>
        /// Request body is form url encoded.
        /// </summary>
        FormEncoded,

        /// <summary>
        /// Request body is binary or a file.
        /// </summary>
        Binary,

        /// <summary>
        /// Request body is multipart form data.
        /// </summary>
        FormData,

        /// <summary>
        /// Request body is plain text.
        /// </summary>
        Text
    }
}