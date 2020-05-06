using MimeMapping;
using Newtonsoft.Json;
using JeniusApps.Nightingale.Core.Common;
using JeniusApps.Nightingale.Core.Workspaces.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Class representing a form data item
    /// used in a request.
    /// </summary>
    public class FormData : ObservableBase, IDeepCloneable<FormData>, IKeyValueToggle
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FormData() { }

        /// <summary>
        /// Constructor that also sets the enabled flag.
        /// </summary>
        public FormData(bool enabled)
        {
            Enabled = enabled;
        }

        /// <inheritdoc/>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <inheritdoc/>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// Content type of the form data item.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }

        /// <summary>
        /// Auto-determined content type.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AutoContentType
        {
            get => _autoContentType;
            set
            {
                if (_autoContentType != value)
                {
                    _autoContentType = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _autoContentType;

        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <summary>
        /// The type of this form data item.
        /// </summary>
        public FormDataType FormDataType
        {
            get => _formDataType;
            set
            {
                if (_formDataType != value)
                {
                    _formDataType = value;
                    RaisePropertyChanged("IsTextType");
                    RaisePropertyChanged("IsFileType");

                    if (value == FormDataType.Text)
                    {
                        AutoContentType = null;
                    }
                    else if (value == FormDataType.File && HasFiles)
                    {
                        AutoContentType = MimeUtility.GetMimeMapping(FilePaths[0]);
                    }
                }
            }
        }
        private FormDataType _formDataType;

        /// <summary>
        /// Helper property for if form data a text type.
        /// </summary>
        [JsonIgnore]
        public bool IsTextType => FormDataType == FormDataType.Text;

        /// <summary>
        /// Helper property for if form data a file type.
        /// </summary>
        [JsonIgnore]
        public bool IsFileType => FormDataType == FormDataType.File;

        /// <summary>
        /// File paths used in the form data if it's set to file type.
        /// </summary>
        public IList<string> FilePaths
        {
            get => _filePaths;
            set
            {
                _filePaths = value;
                RaisePropertyChanged("HasFiles");
                RaisePropertyChanged("HasNoFiles");
                RaisePropertyChanged("SelectedFiles");
            }
        }
        private IList<string> _filePaths;

        /// <summary>
        /// Helper property to quickly get list of files.
        /// </summary>
        [JsonIgnore]
        public string SelectedFiles => HasFiles ? string.Join(", ", FilePaths) : "";

        /// <summary>
        /// Helper property to determine if there are files.
        /// </summary>
        [JsonIgnore]
        public bool HasFiles => FilePaths != null && FilePaths.Count > 0;

        /// <summary>
        /// Helper property to determine if there are no file.
        /// </summary>
        [JsonIgnore]
        public bool HasNoFiles => !HasFiles;

        /// <inheritdoc/>
        public FormData DeepClone()
        {
            return new FormData
            {
                Key = this.Key,
                Value = this.Value,
                ContentType = this.ContentType,
                Enabled = this.Enabled,
                FormDataType = this.FormDataType,
                FilePaths = this.FilePaths?.ToList()
            };
        }
    }
}
