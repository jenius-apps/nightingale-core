namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Class representing a key value pair
    /// with some other properties.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Key of parameter.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Multi-purpose flag for whether or not
        /// this key value pair is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If private, value should not be exported
        /// or transmitted outside of the user's
        /// local device.
        /// </summary>
        public bool Private { get; set; }

        /// <summary>
        /// Type of parameter.
        /// </summary>
        public ParamType Type { get; set; }
    }

    /// <summary>
    /// Type of parameter.
    /// </summary>
    public enum ParamType
    {
        /// <summary>
        /// Generic parameter.
        /// </summary>
        Parameter,

        /// <summary>
        /// Represents an HTTP header.
        /// </summary>
        Header,

        /// <summary>
        /// Represents form encoded data.
        /// </summary>
        FormEncodedData,

        /// <summary>
        /// Represents an environment variable.
        /// </summary>
        EnvVariable,

        /// <summary>
        /// Represents a chaining rule.
        /// </summary>
        ChainingRule
    }
}