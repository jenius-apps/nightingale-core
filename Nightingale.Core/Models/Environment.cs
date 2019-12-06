using Newtonsoft.Json;
using Nightingale.Core.Storage;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Models
{
    public class Environment : ModifiableBase, IStorageItem
    {
        private string _name;
        private bool _isActive;

        public Environment()
        {
        }

        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    ObjectModified();
                }
            }
        }

        public string Icon { get; set; }

        public EnvType EnvironmentType { get; set; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    ObjectModified();
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<Parameter> Variables { get; } = new ObservableCollection<Parameter>();
    }

    public enum EnvType
    {
        Sub,
        Base
    }
}
