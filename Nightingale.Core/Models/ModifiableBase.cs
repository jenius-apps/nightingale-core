using Newtonsoft.Json;

namespace Nightingale.Core.Models
{
    public enum ModifiedStatus
    {
        Unchanged,
        New,
        Modified
    }

    public abstract class ModifiableBase
    {
        [JsonIgnore]
        public ModifiedStatus Status { get; set; }

        protected void ObjectModified()
        {
            if (Status != ModifiedStatus.Modified)
            {
                Status = ModifiedStatus.Modified;
            }
        }

        protected void ObjectModified(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                ObjectModified();
            }
        }
    }
}
