using Newtonsoft.Json;
using Nightingale.Core.Storage;
using System.ComponentModel;

namespace Nightingale.Core.Models
{
    /// <summary>
    /// Abstract class for objects that can be placed
    /// as items in a tree view control.
    /// </summary>
    public abstract class WorkspaceItem : ObservableBase, IStorageItem, IDeepCloneable, INotifyPropertyChanged
    {
        public string Id { get; set; }

        public int Position { get; set; }

        public abstract string Name { get; set; }

        public string ParentId { get; set; }

        public abstract object DeepClone();
    }
}
