using JeniusApps.Nightingale.Data.Models;
using PST = Postman.NET.Collections.Models;

namespace JeniusApps.Nightingale.Converters.Postman
{
    /// <summary>
    /// Interface for converting postman collection
    /// files into Nightingale collection files.
    /// </summary>
    public interface IPostmanConverter
    {
        /// <summary>
        /// Converts a <see cref="PST.Collection"/>
        /// into an <see cref="Item"/>.
        /// </summary>
        /// <param name="postmanCollection">The <see cref="PST.Collection"/> to convert.</param>
        /// <returns>An <see cref="Item"/>.</returns>
        Item? ConvertCollection(PST.Collection postmanCollection);

        /// <summary>
        /// Converts an <see cref="Item"/> 
        /// into a <see cref="PST.Collection"/>.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to convert.</param>
        /// <returns></returns>
        PST.Collection? ConvertToCollection(Item item);
    }
}
