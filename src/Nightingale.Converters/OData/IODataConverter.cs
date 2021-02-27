using JeniusApps.Nightingale.Data.Models;
using Microsoft.OData.Edm;

namespace JeniusApps.Nightingale.Converters.OData
{
    /// <summary>
    /// Interface for converting a OData Metadata document into an <see cref="Item"/>
    /// </summary>
    public interface IODataConverter
    {
        /// <summary>
        /// Converts a <see cref="IEdmModel"/>
        /// into an <see cref="Item"/>.
        /// </summary>
        /// <param name="edmModel">The <see cref="IEdmModel"/> to convert.</param>
        /// <returns>An <see cref="Item"/>.</returns>
        Item? ConvertCollection(IEdmModel edmModel);

        /// <summary>
        /// Converts a <see cref="string"/> that contains a OData metadata xml
        /// into an <see cref="Item"/>.
        /// </summary>
        /// <param name="edmModel">The <see cref="string"/> that is the 
        /// OData metadata xml to convert.</param>
        /// <returns>An <see cref="Item"/>.</returns>
        Item? ConvertCollection(string edmModel);
    }
}
