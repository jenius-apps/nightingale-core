using JeniusApps.Nightingale.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JeniusApps.Nightingale.Data.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Parameter"/>
    /// or lists of them.
    /// </summary>
    public static class ParameterExtensions
    {
        /// <summary>
        /// Returns the parameters which are enabled
        /// and have a valid key.
        /// </summary>
        /// <param name="parameters">List of parameters to search through.</param>
        /// <returns>The enabled parameters.</returns>
        public static IEnumerable<Parameter> GetActive(this IList<Parameter> parameters)
        {
            return parameters
                .Where(x => x.Enabled)
                .Where(x => !string.IsNullOrWhiteSpace(x.Key));
        }
    }
}
