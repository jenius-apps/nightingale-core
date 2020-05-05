using Nightingale.Core.Auth.Enums;
using Nightingale.Core.Workspaces.Models;
using System;
using System.Linq;

namespace Nightingale.Core.Workspaces.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Item"/> class.
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Creates an <see cref="ItemShallowReference"/>.
        /// </summary>
        public static ItemShallowReference ShallowReference(this Item item)
        {
            return new ItemShallowReference(item);
        }

        /// <summary>
        /// Returns the value of the given header key.
        /// Does not resolve variables in the value. Returns null
        /// if header not found.
        /// </summary>
        public static string TryGetHeader(this Item item, string key)
        {
            if (item?.Headers == null || string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            Parameter header = item.Headers
                .GetActive()
                .FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

            return header?.Value;
        }

        /// <summary>
        /// Determines if the given item
        /// is in fact an ancestor of the descendant.
        /// If descendant equals ancestor, false is returned.
        /// If either descendant or ancestor are null, false is returned.
        /// </summary>
        /// <param name="descendant">The item whose ancestor will be checked.</param>
        /// <param name="ancestor">The item that may be the ancestor.</param>
        public static bool IsChildOf(this Item descendant, Item ancestor)
        {
            if (descendant == null || ancestor == null || descendant == ancestor)
            {
                return false;
            }

            // Scout helps break out of loops
            Item scout = descendant.Parent?.Parent;
            Item current = descendant.Parent;

            while (current != null && current != scout)
            {
                if (current == ancestor)
                {
                    return true;
                }

                scout = scout?.Parent?.Parent;
                current = current.Parent;
            }

            return false;
        }

        /// <summary>
        /// Searches for the top-level auth in the inheritance tree.
        /// </summary>
        /// <param name="item">The item whose auth will be determined.</param>
        /// <returns>The inherited authentication.</returns>
        public static Authentication GetAuthInheritance(this Item item)
        {
            if (item?.Auth == null)
            {
                return null;
            }

            Item currentItem = item;
            while (currentItem?.Auth?.AuthType == AuthType.InheritParent
                && currentItem.Parent != null)
            {
                currentItem = currentItem.Parent;
            }

            return currentItem?.Auth;
        }
    }
}
