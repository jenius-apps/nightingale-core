using JeniusApps.Nightingale.Data.Models;
using Microsoft.OpenApi.Models;

namespace JeniusApps.Nightingale.Converters.Swagger
{
    /// <summary>
    /// Interface for converting swagger/openapi documents
    /// to nightingale items.
    /// </summary>
    public interface ISwaggerConverter
    {
        /// <summary>
        /// Converts the given swagger document
        /// to a workspace item.
        /// </summary>
        Item ConvertSwaggerDoc(OpenApiDocument document);
    }
}