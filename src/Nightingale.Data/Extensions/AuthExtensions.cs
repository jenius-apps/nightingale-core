using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;

namespace JeniusApps.Nightingale.Data.Extensions
{
    /// <summary>
    /// Helper methods for manipulation
    /// <see cref="Authentication"/>.
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// Retrieves a string property from the property
        /// dictionary of Authentication.
        /// </summary>
        /// <param name="auth">The <see cref="Authentication"/> to inspect.</param>
        /// <param name="key">The key of the property to retrieve.</param>
        /// <returns>The value of the property.</returns>
        public static string GetProp(this Authentication auth, string key)
        {
            if (auth?.AuthProperties == null)
            {
                return "";
            }

            if (auth.AuthProperties.TryGetValue(key, out string value))
            {
                return value ?? "";
            }

            return "";
        }

        /// <summary>
        /// Retrieves an enum property from the property
        /// dictionary of Authentication.
        /// </summary>
        /// <param name="auth">The <see cref="Authentication"/> to inspect.</param>
        /// <param name="key">The key of the property to retrieve.</param>
        /// <returns>The value of the property.</returns>
        public static TEnum GetEnumProp<TEnum>(this Authentication auth, string key) where TEnum : struct
        {
            var value = auth.GetProp(key);

            if (Enum.TryParse<TEnum>(value, out var result))
            {
                return result;
            }

            return default;
        }

        /// <summary>
        /// Sets the given property.
        /// </summary>
        /// <param name="auth">The <see cref="Authentication"/> to inspect.</param>
        /// <param name="key">The property's name.</param>
        /// <param name="value">The property's value.</param>
        public static void SetProp(this Authentication auth, string key, string value)
        {
            if (auth == null)
            {
                return;
            }

            if (auth.AuthProperties == null)
            {
                auth.AuthProperties = new Dictionary<string, string>();
            }

            if (string.IsNullOrEmpty(value))
            {
                // delete the prop
                auth.AuthProperties.Remove(key);
                return;
            }

            if (auth.AuthProperties.ContainsKey(key))
            {
                auth.AuthProperties[key] = value;
            }
            else
            {
                auth.AuthProperties.Add(key, value);
            }
        }
    }
}
