using Newtonsoft.Json;
using JeniusApps.Nightingale.Core.Common;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// An object that holds a key value pair
    /// and can also be enabled or disabled.
    /// </summary>
    public class Parameter : ObservableBase, IDeepCloneable<Parameter>, IKeyValueToggle
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// This was specifically defined to
        /// enable serialization.
        /// </remarks>
        public Parameter() { }

        /// <summary>
        /// Constructs using given values.
        /// </summary>
        public Parameter(
            bool isEnabled,
            string key,
            string value)
        {
            Enabled = isEnabled;
            Key = key;
            Value = value;
        }

        /// <inheritdoc/>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _key;

        /// <inheritdoc/>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _value;

        /// <inheritdoc/>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool _enabled;

        /// <inheritdoc/>
        public Parameter DeepClone()
        {
            var result = new Parameter
            {
                Key = this.Key,
                Value = this.Value,
                Enabled = this.Enabled
            };

            return result;
        }

        public override string ToString()
        {
            return $"{this.Key}={this.Value}";
        }
    }
}
