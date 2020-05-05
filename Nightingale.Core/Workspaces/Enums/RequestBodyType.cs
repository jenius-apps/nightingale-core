namespace Nightingale.Core.Workspaces.Enums
{
    /// <summary>
    /// The type of request body used.
    /// </summary>
    public enum RequestBodyType
    {
        /// <summary>
        /// Default, no content.
        /// </summary>
        None,

        /// <summary>
        /// JSON content.
        /// </summary>
        Json,

        /// <summary>
        /// XML content.
        /// </summary>
        Xml,

        /// <summary>
        /// Form encoded content.
        /// </summary>
        FormEncoded,

        /// <summary>
        /// Binary file content.
        /// </summary>
        Binary,

        /// <summary>
        /// Multipart form data content.
        /// </summary>
        FormData,

        /// <summary>
        /// Plain text content.
        /// </summary>
        Text
    }
}
