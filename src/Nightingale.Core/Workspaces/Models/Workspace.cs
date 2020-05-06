using Newtonsoft.Json;
using JeniusApps.Nightingale.Core.Common;
using JeniusApps.Nightingale.Core.Workspaces.EventHandlers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// Class that holds all the data for one
    /// workspace in Nightingale. This includes
    /// the environment variables, the list of requests
    /// and collections, and more.
    /// </summary>
    public class Workspace : ObservableBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Workspace()
        {
            // This updates the parent property of each
            // new child to be null because this is the root.
            Items.CollectionChanged += (sender, e) => ItemEventHandlers.CollectionChanged(sender, e, null);
        }

        /// <summary>
        /// Id of item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id of parent.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Name of workspace.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string _name;


        /// <summary>
        /// The list of HTTP methods configured
        /// for this workspace.
        /// </summary>
        public List<string> Methods { get; set; }

        /// <summary>
        /// List of requests or collections in this workspace.
        /// </summary>
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        /// <summary>
        /// List of request items that were previously sent by the user.
        /// </summary>
        public ObservableCollection<HistoryItem> HistoryItems { get; } = new ObservableCollection<HistoryItem>();

        /// <summary>
        /// List of environments for this workspace.
        /// </summary>
        public ObservableCollection<Env> Environments { get; } = new ObservableCollection<Env>();

        /// <summary>
        /// List of cookies preserved for this workspace.
        /// </summary>
        public ObservableCollection<Cookie> WorkspaceCookies { get; } = new ObservableCollection<Cookie>();

        /// <summary>
        /// The currently selected workspace item.
        /// </summary>
        [JsonIgnore]
        public Item CurrentItem
        {
            get => _currentItem;
            set
            {
                if (value != _currentItem)
                {
                    _currentItem = value;
                    RaisePropertyChanged();
                }
            }
        }
        private Item _currentItem;

        public override string ToString()
        {
            return this.Name;
        }
    }
}
