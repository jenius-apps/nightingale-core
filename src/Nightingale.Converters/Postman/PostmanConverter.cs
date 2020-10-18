using JeniusApps.Nightingale.Data.Extensions;
using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PST = Postman.NET.Collections.Models;

namespace Nightingale.Converters.Postman
{
    /// <summary>
    /// Class for converting Postman collections into Nightingale collections.
    /// </summary>
    public class PostmanConverter : IPostmanConverter
    {
        public static readonly string[] SupportedSchemas = new string[]
        {
            "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
        };

        /// <inheritdoc/>
        public Item ConvertCollection(PST.Collection postmanCollection)
        {
            if (postmanCollection == null)
            {
                return null;
            }

            var collection = new Item
            {
                Type = ItemType.Collection,
                Name = postmanCollection.Info?.Name
            };

            IList<Item> children = ConvertItems(postmanCollection.Items);
            if (children != null)
            {
                foreach (var child in children)
                {
                    collection.Children.Add(child);
                }
            }

            return collection;
        }

        private IList<Item> ConvertItems(PST.Item[] postmanItems)
        {
            if (postmanItems == null || postmanItems.Length == 0)
            {
                return new List<Item>();
            }

            var list = new List<Item>();

            foreach (var item in postmanItems)
            {
                if (item.Request != null)
                {
                    // item is a request
                    Item request = ConvertRequest(item.Request);

                    if (request != null)
                    {
                        request.Name = item.Name;
                        list.Add(request);
                    }
                }
                else if (item.Items != null && item.Items.Length > 0)
                {
                    IList<Item> childCollections = ConvertItems(item.Items);
                    list.AddRange(childCollections);
                }
            }

            return list;
        }

        private RequestBody ConvertBody(PST.Body body)
        {
            if (body == null)
            {
                return new RequestBody();
            }

            var result = new RequestBody();

            if (body.Mode == "raw")
            {
                // Set raw body text
                switch (body.Options?.Raw?.Language)
                {
                    case "json":
                        result.JsonBody = body.Raw;
                        result.BodyType = RequestBodyType.Json;
                        break;
                    case "xml":
                        result.XmlBody = body.Raw;
                        result.BodyType = RequestBodyType.Xml;
                        break;
                    default:
                        result.TextBody = body.Raw;
                        result.BodyType = RequestBodyType.Text;
                        break;
                }
            }
            else if (body.Mode == "urlencoded" && body.Urlencoded != null)
            {
                result.BodyType = RequestBodyType.FormEncoded;

                foreach (var postmanUrlEncoded in body.Urlencoded)
                {
                    var parameter = new Parameter
                    {
                        Type = ParamType.FormEncodedData,
                        Key = postmanUrlEncoded.Key,
                        Value = postmanUrlEncoded.Value
                    };

                    result.FormEncodedData?.Add(parameter);
                }
            }
            else if (body.Mode == "formdata" && body.Formdata != null)
            {
                result.BodyType = RequestBodyType.FormData;

                foreach (var postmanForm in body.Formdata)
                {
                    var nightingaleForm = new FormData
                    {
                        ContentType = postmanForm.ContentType,
                        Key = postmanForm.Key,
                        Value = postmanForm.Value,
                        FilePaths = new List<string>(),
                        Enabled = !postmanForm.Disabled
                    };

                    if (postmanForm.Type == "text")
                    {
                        nightingaleForm.FormDataType = FormDataType.Text;
                    }
                    else if (postmanForm.Type == "file")
                    {
                        nightingaleForm.FormDataType = FormDataType.File;
                    }

                    if (postmanForm.Src is string[] src)
                    {
                        foreach (var s in src)
                        {
                            if (string.IsNullOrWhiteSpace(s))
                            {
                                continue;
                            }

                            nightingaleForm.FilePaths.Add(s.TrimStart('/'));
                        }
                    }

                    result.FormDataList?.Add(nightingaleForm);
                }
            }
            else if (body.Mode == "file" && body.File?.Src != null)
            {
                result.BinaryFilePath = body.File.Src.TrimStart('/');
                result.BodyType = RequestBodyType.Binary;
            }

            return result;
        }

        private Authentication ConvertAuth(PST.Auth postmanAuth)
        {
            if (postmanAuth == null)
            {
                return new Authentication();
            }

            var result = new Authentication();

            // Basic
            result.SetProp(AuthConstants.BasicPassword, postmanAuth.Basic?.FirstOrDefault(x => x.Key == "password")?.Value);
            result.SetProp(AuthConstants.BasicUsername, postmanAuth.Basic?.FirstOrDefault(x => x.Key == "username")?.Value);

            // Oauth 1
            result.SetProp(AuthConstants.OAuth1ConsumerKey, postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "consumerKey")?.Value);
            result.SetProp(AuthConstants.OAuth1ConsumerSecret, postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "consumerSecret")?.Value);
            result.SetProp(AuthConstants.OAuth1AccessToken, postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "token")?.Value);
            result.SetProp(AuthConstants.OAuth1TokenSecret, postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "tokenSecret")?.Value);

            // Oauth 2
            result.SetProp(AuthConstants.OAuth2AccessToken, postmanAuth.Oauth2?.FirstOrDefault(x => x.Key == "accessToken")?.Value);

            // Bearer
            result.SetProp(AuthConstants.BearerToken, postmanAuth.Bearer?.FirstOrDefault(x => x.Key == "token")?.Value);

            // Digest
            result.SetProp(AuthConstants.DigestPassword, postmanAuth.Digest?.FirstOrDefault(x => x.Key == "password")?.Value);
            result.SetProp(AuthConstants.DigestUsername, postmanAuth.Digest?.FirstOrDefault(x => x.Key == "username")?.Value);


            if (Enum.TryParse(postmanAuth.Type, out PST.PostmanAuthType postmanAuthType)
                && Enum.IsDefined(typeof(AuthType), (int)postmanAuthType))
            {
                result.AuthType = (AuthType)postmanAuthType;
            }

            return result;
        }

        private Item ConvertRequest(PST.Request postmanRequest)
        {
            if (postmanRequest == null)
            {
                return null;
            }

            var result = new Item
            {
                Type = ItemType.Request,
                Url = new Url
                {
                    Base = postmanRequest.Url?.Raw
                },
                Body = ConvertBody(postmanRequest.Body),
                Method = postmanRequest.Method,
                Auth = ConvertAuth(postmanRequest.Auth)
            };

            // Headers
            if (postmanRequest.Header != null && postmanRequest.Header.Length > 0)
            {
                foreach (var header in postmanRequest.Header)
                {
                    result.Headers.Add(new Parameter
                    {
                        Enabled = !header.Disabled,
                        Key = header.Key,
                        Value = header.Value,
                        Type = ParamType.Header
                    });
                }
            }

            // Queries
            if (postmanRequest.Url?.Query != null)
            {
                foreach (var query in postmanRequest.Url.Query)
                {
                    result.Url.Queries.Add(new Parameter
                    {
                        Enabled = !query.Disabled,
                        Key = query.Key,
                        Value = query.Value,
                        Type = ParamType.Header
                    });
                }
            }

            return result;
        }
    }
}
