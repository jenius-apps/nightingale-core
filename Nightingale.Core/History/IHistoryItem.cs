using System;

namespace Nightingale.Core.History
{
    public interface IHistoryItem
    {
        DateTime LastUsedDate { get; set; }
    }
}
