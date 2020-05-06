namespace JeniusApps.Nightingale.Core.Workspaces.Enums
{
    /// <summary>
    /// Describes if the environment is a 
    /// substitute or the base environment.
    /// </summary>
    public enum EnvType
    {
        /// <summary>
        /// This environment is a substitute. It was user-created
        /// and is not the base environment. Thus
        /// it can be deleted, and many of them can be added.
        /// </summary>
        Sub,

        /// <summary>
        /// The base environment is the default
        /// for a workspace. Every workspace must have
        /// exactly 1 base environment.
        /// </summary>
        Base
    }
}
