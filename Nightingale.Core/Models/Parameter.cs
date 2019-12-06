using Nightingale.Core.Storage;

namespace Nightingale.Core.Models
{
    public class Parameter : ModifiableBase, IParameter, IStorageItem, IDeepCloneable
    {
        private string _key;
        private string _value;
        private bool _enabled;

        public Parameter()
        {
        }

        public Parameter(bool isEnabled = true, string key = "", string value = "", ParamType type = ParamType.Parameter)
        {
            Enabled = isEnabled;
            Key = key;
            Value = value;
            Type = type;
            Status = ModifiedStatus.New;
        }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    ObjectModified();
                }
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ObjectModified();
                }
            }
        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    ObjectModified();
                }
            }
        }

        public ParamType Type { get; set; }

        public object DeepClone()
        {
            var result = new Parameter
            {
                Key = this.Key,
                Value = this.Value,
                Enabled = this.Enabled,
                Type = this.Type,
                Status = ModifiedStatus.New
            };

            return result;
        }
    }

    public enum ParamType
    {
        Parameter,
        Header,
        FormEncodedData,
        EnvVariable,
        ChainingRule
    }
}
