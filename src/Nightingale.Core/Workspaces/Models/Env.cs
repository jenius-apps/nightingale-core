using JeniusApps.Nightingale.Core.Common;
using JeniusApps.Nightingale.Core.Workspaces.Enums;
using JeniusApps.Nightingale.Core.Workspaces.Extensions;
using System.Collections.ObjectModel;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// An object that contains variables
    /// which will be used to replace
    /// variable strings upon sending a request.
    /// </summary>
    public class Env : ObservableBase, IDeepCloneable<Env>
    {
        /// <summary>
        /// Name of the environment.
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
        /// Defines if an environment is the base or a substitute.
        /// </summary>
        public EnvType EnvironmentType { get; set; }

        /// <summary>
        /// Determines if the environment is currently active.
        /// There should only be 1 active environment at a time
        /// in a workspace.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged();
                }
            }
        }
        private bool _isActive;

        /// <summary>
        /// List of variables in the environment.
        /// </summary>
        public ObservableCollection<Parameter> Variables { get; } = new ObservableCollection<Parameter>();

        /// <inheritdoc/>
        public Env DeepClone()
        {
            var other = new Env
            {
                Name = this.Name,
                EnvironmentType = EnvType.Sub,
                IsActive = false,
            };
            other.Variables.DeepClone(this.Variables);
            return other;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
