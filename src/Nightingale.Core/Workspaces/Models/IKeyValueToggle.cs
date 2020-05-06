namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// An interface for items containing a
    /// key value pair which can be toggled on
    /// or off.
    /// </summary>
    public interface IKeyValueToggle
    {
        /// <summary>
        /// Flag for enabling or disabling this KVP.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Key of this item.
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Value of this item.
        /// </summary>
        string Value { get; set; }
    }
}
