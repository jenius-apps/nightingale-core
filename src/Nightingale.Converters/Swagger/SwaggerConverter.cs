using JeniusApps.Nightingale.Data.Models;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nightingale.Converters.Swagger
{
    /// <summary>
    /// Class for converting swagger/openapi
    /// documents to nightingale items.
    /// </summary>
    public class SwaggerConverter : ISwaggerConverter
    {
        public static readonly string[] SupportedSchemas = new string[]
        {
            "3.0.0"
        };

        /// <inheritdoc/>
        public Item ConvertSwaggerDoc(OpenApiDocument document)
        {
            if (document == null)
            {
                return null;
            }

            var collection = new Item
            {
                Type = ItemType.Collection,
                Name = document.Info.Title
            };

            string serverUrl = document.Servers?.FirstOrDefault()?.Url;

            foreach (var path in document.Paths)
            {
                var children = GetChildren(path, serverUrl);

                if (children == null || children.Count == 0)
                {
                    continue;
                }

                foreach (var child in children)
                {
                    collection.Children.Add(child);
                }
            }

            return collection;
        }

        private IList<Item> GetChildren(KeyValuePair<string, OpenApiPathItem> path, string serverUrl)
        {
            var result = new List<Item>();

            foreach (var operation in path.Value.Operations)
            {
                var request = new Item
                {
                    Type = ItemType.Request,
                    Url = new Url
                    {
                        Base = serverUrl + path.Key
                    },
                    Name = operation.Value.Summary,
                    Method = operation.Key.ToString()
                };

                foreach (var parameter in operation.Value.Parameters)
                {
                    if (parameter.In == ParameterLocation.Query)
                    {
                        request.Url.Queries.Add(new Parameter
                        {
                            Key = parameter.Name,
                            Value = parameter.Schema?.Type,
                            Enabled = true,
                            Type = ParamType.Parameter
                        });
                    }
                    else if (parameter.In == ParameterLocation.Header)
                    {
                        request.Headers.Add(new Parameter
                        {
                            Key = parameter.Name,
                            Value = parameter.Schema?.Type,
                            Enabled = true,
                            Type = ParamType.Header
                        });
                    }
                }

                result.Add(request);
            }

            return result;
        }
    }
}
