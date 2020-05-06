using Newtonsoft.Json;
using Nightingale.Core.Common;
using Nightingale.Core.Workspaces.Enums;
using Nightingale.Core.Workspaces.Extensions;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Object representing a request body.
    /// </summary>
    public class RequestBody : IDeepCloneable<RequestBody>
    {
        /// <summary>
        /// The current type of body as specified
        /// by the user.
        /// </summary>
        public RequestBodyType BodyType { get; set; }

        /// <summary>
        /// The JSON text body.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string JsonBody { get; set; }

        /// <summary>
        /// The XML text body.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string XmlBody { get; set; }

        /// <summary>
        /// The plain text body.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextBody { get; set; }

        /// <summary>
        /// The form endcoded data body.
        /// </summary>
        public ObservableCollection<Parameter> FormEncodedData { get; } = new ObservableCollection<Parameter>();

        /// <summary>
        /// The multipart form data body.
        /// </summary>
        public ObservableCollection<FormData> FormDataList { get; } = new ObservableCollection<FormData>();

        /// <summary>
        /// The binary file body.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BinaryFilePath { get; set; }

        /// <inheritdoc/>
        public RequestBody DeepClone()
        {
            var other = new RequestBody()
            {
                BodyType = this.BodyType,
                JsonBody = this.JsonBody,
                XmlBody = this.XmlBody,
                BinaryFilePath = this.BinaryFilePath,
                TextBody = this.TextBody,
            };

            other.FormDataList.DeepClone(this.FormDataList);
            other.FormEncodedData.DeepClone(this.FormEncodedData);

            return other;
        }
    }
}
