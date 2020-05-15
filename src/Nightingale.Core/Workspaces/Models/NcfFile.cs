using System.Collections.Generic;

namespace JeniusApps.Nightingale.Core.Workspaces.Models
{
    /// <summary>
    /// The object model for a Nightingale
    /// Collection Format file.
    /// </summary>
    public class NcfFile
    {
        /// <summary>
        /// The list of workspaces in the file.
        /// </summary>
        public List<Workspace> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new List<Workspace>();
                }

                return _workspaces;
            }
            set => _workspaces = value;
        }
        private List<Workspace> _workspaces;

        /// <summary>
        /// List of recently accessed URLs.
        /// </summary>
        public List<RecentUrl> RecentUrls
        {
            get
            {
                if (_recentUrls == null)
                {
                    _recentUrls = new List<RecentUrl>();
                }

                return _recentUrls;
            }
            set => _recentUrls = value;
        }
        private List<RecentUrl> _recentUrls;
    }
}
