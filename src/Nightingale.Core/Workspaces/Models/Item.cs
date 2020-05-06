using Newtonsoft.Json;
using JeniusApps.Nightingale.Core.Common;
using JeniusApps.Nightingale.Core.Workspaces.Enums;
using JeniusApps.Nightingale.Core.Workspaces.EventHandlers;
using JeniusApps.Nightingale.Core.Workspaces.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// This workspace item represents an object that
    /// can be displayed in the tree of the workspace.
    /// </summary>
    public class Item : ObservableBase, IEquatable<Item>, IComparable<Item>, IDeepCloneable<Item>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Item()
        {
            // This updates the Parent properties
            // of all children added to this Item.
            Children.CollectionChanged += (sender, e) => ItemEventHandlers.CollectionChanged(sender, e, this);
        }

        /// <summary>
        /// Constructor that specifies of the children collection
        /// will be observed for changes.
        /// </summary>
        /// <param name="childenObservable">
        /// If true, changes in the children collection will be observed and
        /// associated logic will be triggered. If false, the collection's changes
        /// will not be observed.
        /// </param>
        public Item(bool childenObservable)
        {
            if (childenObservable)
            {
                // This updates the Parent properties
                // of all children added to this Item.
                Children.CollectionChanged += (sender, e) => ItemEventHandlers.CollectionChanged(sender, e, this);
            }
        }

        /// <summary>
        /// A reference to the parent of this item.
        /// </summary>
        [JsonIgnore]
        public Item Parent { get; set; }

        /// <summary>
        /// A dictionary of properties usually
        /// used by a GUI app.
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Url of this item. Usually only used
        /// by requests.
        /// </summary>
        public Url Url { get; set; } = new Url();

        /// <summary>
        /// The authentication properties of this item.
        /// </summary>
        public Authentication Auth { get; set; } = new Authentication();

        /// <summary>
        /// The request body for this item. Usually only used
        /// by requests.
        /// </summary>
        public RequestBody Body { get; set; } = new RequestBody();

        /// <summary>
        /// The children items of this item.
        /// </summary>
        public ObservableCollection<Item> Children { get; } = new ObservableCollection<Item>();

        /// <summary>
        /// The headers of this item set by the user.
        /// </summary>
        public ObservableCollection<Parameter> Headers { get; } = new ObservableCollection<Parameter>();

        /// <summary>
        /// Chaining rules for this item set by the user.
        /// </summary>
        public ObservableCollection<Parameter> ChainingRules { get; } = new ObservableCollection<Parameter>();

        /// <summary>
        /// The item's current type.
        /// </summary>
        public ItemType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    RaisePropertyChanged();
                }
            }
        }
        private ItemType _type;

        /// <summary>
        /// The item's name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        private string _name;

        /// <summary>
        /// Helper boolean for GUI apps
        /// to specify if this item's list of children
        /// is displayed.
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool _isExpanded;

        /// <summary>
        /// The http method of this item used
        /// when sending a request.
        /// </summary>
        public string Method
        {
            get => _method;
            set
            {
                _method = value;
                RaisePropertyChanged();
            }
        }
        private string _method;

        /// <summary>
        /// The http response to this item.
        /// </summary>
        public Response Response
        {
            get => _response;
            set
            {
                _response = value;
                RaisePropertyChanged();
            }
        }
        private Response _response;

        public override int GetHashCode()
        {
            return Method.GetHashCode() * 0x00010000
                * Name.GetHashCode() * 0x00010000
                * Type.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Item other)
        {
            if (other is null) return 1;

            return Name.CompareTo(other.Name);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Item);
        }

        public bool Equals(Item other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other);
        }

        /// <inheritdoc/>
        public virtual Item DeepClone()
        {
            var other = new Item
            {
                Parent = this.Parent,
                Url = this.Url?.DeepClone(),
                Auth = this.Auth?.DeepClone(),
                Body = this.Body?.DeepClone(),
                Response = this.Response?.DeepClone(),
                Type = this.Type,
                Name = this.Name,
                IsExpanded = this.IsExpanded,
                Method = this.Method
            };
            other.Headers.DeepClone(this.Headers);
            other.ChainingRules.DeepClone(this.ChainingRules);
            other.Children.DeepClone(this.Children);
            return other;
        }

        public static bool operator ==(Item left, Item right)
        {
            // Check for null on left side.
            if (left is null)
            {
                if (right is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Item left, Item right)
        {
            return !(left == right);
        }
    }
}
