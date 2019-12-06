using Nightingale.Core.Storage;

namespace Nightingale.Core.Models
{
    public interface IParameter : IStorageItem
    {
        string Key { get; set; }

        string Value { get; set; }

        bool Enabled { get; set; }
    }
}
