using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Models
{
    /// <summary>
    /// Nightingale Collection Format file.
    /// </summary>
    public class NcfFile
    {
        /// <summary>
        /// ID of the active workspace.
        /// </summary>
        public string ActiveWorkspaceId { get; set; }

        /// <summary>
        /// List of workspaces
        /// </summary>
        public List<Workspace> Workspaces { get; set; }
    }
}
