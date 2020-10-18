using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Class representing form data
    /// for a request's multipart form body.
    /// </summary>
    public class FormData
    {
        /// <summary>
        /// Key string for form data.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of form data if it's not a file.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Multi-purpose flag for whether or not
        /// this form data is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// HTTP content type defined by the user.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// List of paths for this form data
        /// to attach.
        /// </summary>
        public IList<string> FilePaths { get; set; }

        /// <summary>
        /// The type of form data.
        /// </summary>
        public FormDataType FormDataType { get; set; }

        /// <summary>
        /// The default content type for this form data.
        /// Usually set when selecting a file. The file's
        /// content type should be assigned here.
        /// </summary>
        public string AutoContentType { get; set; }
    }

    /// <summary>
    /// The type of form data.
    /// </summary>
    public enum FormDataType
    {
        /// <summary>
        /// Form data contains text.
        /// </summary>
        Text,

        /// <summary>
        /// Form data represents files.
        /// </summary>
        File
    }
}