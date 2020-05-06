using Nightingale.Core.Workspaces.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nightingale.Core.Workspaces.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IKeyValueToggle"/>
    /// or lists of them.
    /// </summary>
    public static class KeyValueToggleExtensions
    {
        /// <summary>
        /// Returns list of active items.
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IKeyValueToggle"/>.</typeparam>
        /// <param name="list">The list to query.</param>
        /// <param name="keyMustBeValid">If true, filters results to items with a non-empty and non-whitespace key.</param>
        /// <returns>Returns list of active items.</returns>
        public static IEnumerable<T> GetActive<T>(
            this IList<T> list,
            bool keyMustBeValid = true)
            where T : IKeyValueToggle
        {
            return keyMustBeValid
                ? list.Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.Key))
                : list.Where(x => x.Enabled);
        }
    }
}
