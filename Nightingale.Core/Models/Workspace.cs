using Newtonsoft.Json;
using Nightingale.Core.Storage;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Models
{
    public class Workspace : ObservableBase, IStorageItem
    {
        private string _name;

        public Workspace()
        {
        }

        public string Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string ParentId { get; set; }

        [JsonIgnore]
        public ObservableCollection<WorkspaceItem> WorkspaceItems { get; } = new ObservableCollection<WorkspaceItem>();

        [JsonIgnore]
        public ObservableCollection<Environment> WorkspaceEnvironments { get; } = new ObservableCollection<Environment>();

        public ObservableCollection<Cookies.Models.Cookie> WorkspaceCookies { get; } = new ObservableCollection<Cookies.Models.Cookie>();

        [JsonIgnore]
        public WorkspaceItem SelectedItem { get; set; }
    }
}
