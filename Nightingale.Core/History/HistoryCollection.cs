using Nightingale.Core.Models;
using System;

namespace Nightingale.Core.History
{
    public class HistoryCollection : WorkspaceCollection, IHistoryItem
    {
        public HistoryCollection()
        {
        }

        public DateTime LastUsedDate { get; set; }
    }
}
