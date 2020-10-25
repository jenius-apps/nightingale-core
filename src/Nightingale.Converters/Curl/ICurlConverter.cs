using JeniusApps.Nightingale.Data.Models;

namespace JeniusApps.Nightingale.Converters.Curl
{
    /// <summary>
    /// Interface for converting a curl command
    /// to an <see cref="Item"/>
    /// </summary>
    public interface ICurlConverter
    {
        /// <summary>
        /// Converts a curl string to an <see cref="Item"/>.
        /// </summary>
        /// <param name="curlString">The curl string to convert.</param>
        /// <returns>A request or collection in the form of a <see cref="Item"/>.</returns>
        Item Convert(string curlString);
    }
}
