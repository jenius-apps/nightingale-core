using Nightingale.Core.Storage;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Models
{
    public interface IWorkspaceCollection : IStorageItem
    {
        string Name { get; set; }

        bool IsExpanded { get; set; }

        int Position { get; set; }

        ObservableCollection<WorkspaceItem> Children { get; }
    }
}
