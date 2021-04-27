﻿using JeniusApps.Nightingale.Data.Extensions;
using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PST = Postman.NET.Collections.Models;

namespace JeniusApps.Nightingale.Converters.Postman
{
    /// <summary>
    /// Class for converting Postman collections into Nightingale collections and convert back Nightingale collection to postman collections
    /// </summary>
    public class PostmanConverter : IPostmanConverter
    {
        /// <summary>
        /// Scheme versions or json supported by this converter.
        /// </summary>
        public static readonly string[] SupportedSchemas = new string[]
        {
            "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
        };

        /// <inheritdoc/>
        public Item? ConvertCollection(PST.Collection postmanCollection)
        {
            if (postmanCollection == null)
            {
                return null;
            }

            var collection = new Item
            {
                Type = ItemType.Collection,
                Name = postmanCollection.Info?.Name,
                Children = new List<Item>()
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

        public PST.Collection ConvertCollection(Item nightingaleCollection)
        {
            if (nightingaleCollection == null)
            {
                return null;
            }

            PST.Collection postmanCollection = new PST.Collection()
            {
                Info = new PST.Info()
                {
                    Name = nightingaleCollection.Name
                },
                Items = ConvertItems(nightingaleCollection.Children)
            };

            return postmanCollection;
        }

        private PST.Item[] ConvertItems(List<Item> nightingaleCollectionChildren)
        {
            if (nightingaleCollectionChildren == null || nightingaleCollectionChildren.Count == 0)
            {
                return new PST.Item[0];
            }

            return nightingaleCollectionChildren.Select(item => item.Type switch
            {
                ItemType.Request => new PST.Item()
                {
                    Name = item.Name,
                    Request = ConvertRequest(item)
                },
                ItemType.Collection => new PST.Item()
                {
                    Name = item.Name,
                    Items = ConvertItems(item.Children)
                }
            }).ToArray();
        }

        private PST.Request? ConvertRequest(Item nightingaleRequest)
        {
            if (nightingaleRequest == null)
            {
                return null;
            }
            
            return new PST.Request()
                {
                    Url = new PST.Url()
                    {
                        Raw = nightingaleRequest.Url.Base,
                        Query = ConvertQuery(nightingaleRequest.Url.Queries)
                    },
                    Body = ConvertBody(nightingaleRequest.Body),
                    Method = nightingaleRequest.Method,
                    Auth = ConvertAuth(nightingaleRequest.Auth),
                    Header = ConvertHeaders(nightingaleRequest.Headers),
                };
        }


        private static PST.Query[] ConvertQuery(IEnumerable<Parameter> urlQueries) => urlQueries.Select(urlQuery =>
            new PST.Query()
            {
                Disabled = !urlQuery.Enabled,
                Key = urlQuery.Key,
                Value = urlQuery.Value
            }).ToArray();

        private static PST.Parameter[]? ConvertHeaders(IReadOnlyCollection<Parameter> nightingaleRequestHeaders)
        {
            if (nightingaleRequestHeaders is {Count: 0})
            {
                return null;
            }

            return nightingaleRequestHeaders.Select(nightingaleParam => new PST.Parameter()
            {
                Disabled = !nightingaleParam.Enabled,
                Key = nightingaleParam.Key,
                Value = nightingaleParam.Value,
                Type = ParamType.Header.ToString()
            }).ToArray();
        }

        private PST.Auth ConvertAuth(Authentication nightingaleRequestAuth)
        {
            if (nightingaleRequestAuth != null)
            {
                return new PST.Auth();
            }

            var result = new PST.Auth();

            // Basic
            var username = new PST.Parameter()
                {Key = "username", Value = nightingaleRequestAuth.GetProp(AuthConstants.BasicUsername)};
            var password = new PST.Parameter()
                {Key = "password", Value = nightingaleRequestAuth.GetProp(AuthConstants.BasicPassword)};
            result.Basic = new PST.Parameter[] {username, password};

            // OAuth 1
            var consumerKey = new PST.Parameter()
                {Key = "consumerKey", Value = nightingaleRequestAuth.GetProp(AuthConstants.OAuth1ConsumerKey)};
            var consumerSecret = new PST.Parameter()
                {Key = "consumerSecret", Value = nightingaleRequestAuth.GetProp(AuthConstants.OAuth1ConsumerSecret)};
            var token = new PST.Parameter()
                {Key = "token", Value = nightingaleRequestAuth.GetProp(AuthConstants.OAuth1AccessToken)};
            var tokenSecret = new PST.Parameter()
                {Key = "tokenSecret", Value = nightingaleRequestAuth.GetProp(AuthConstants.OAuth1TokenSecret)};
            result.Oauth1 = new PST.Parameter[] {consumerKey, consumerSecret, token, tokenSecret};

            // OAuth 2
            var accessTokenOAuth2 = new PST.Parameter()
                {Key = "accessToken", Value = nightingaleRequestAuth.GetProp(AuthConstants.BasicUsername)};
            result.Oauth2 = new PST.Parameter[] {accessTokenOAuth2};

            // Digest
            var digestUsername = new PST.Parameter()
                {Key = "username", Value = nightingaleRequestAuth.GetProp(AuthConstants.DigestUsername)};
            var digestPassword = new PST.Parameter()
                {Key = "password", Value = nightingaleRequestAuth.GetProp(AuthConstants.DigestPassword)};
            result.Digest = new PST.Parameter[] {digestUsername, digestPassword};

            result.Type = nightingaleRequestAuth.AuthType.ToString();

            return result;
        }

        private PST.Body ConvertBody(RequestBody nightingaleRequestBody)
        {
            if (nightingaleRequestBody == null)
            {
                return null;
            }

            var result = new PST.Body();

            switch (nightingaleRequestBody.BodyType)
            {
                case RequestBodyType.FormEncoded:
                    result.Mode = "urlencoded";
                    result.Urlencoded = nightingaleRequestBody.FormEncodedData.Select(param => new PST.Parameter()
                    {
                        Key = param.Key,
                        Value = param.Value
                    }).ToArray();
                    break;
                case RequestBodyType.FormData:
                    result.Mode = "formdata";
                    result.Formdata = nightingaleRequestBody.FormDataList.Select(param => new PST.FormData()
                    {
                        Key = param.Key,
                        Value = param.Value,
                        ContentType = param.ContentType,
                        Disabled = !param.Enabled,
                        Type = param.FormDataType == FormDataType.File ? "file" : "text",
                        Src = param.FilePaths?.ToArray()
                    }).ToArray();
                    break;
                case RequestBodyType.Binary:
                    result.File.Src = nightingaleRequestBody.BinaryFilePath;
                    break;
                case RequestBodyType.Json:
                    result.Options.Raw.Language = "json";
                    result.Raw = nightingaleRequestBody.JsonBody;
                    break;
                case RequestBodyType.Xml:
                    result.Options.Raw.Language = "xml";
                    result.Raw = nightingaleRequestBody.XmlBody;
                    break;
                case RequestBodyType.Text:
                    result.Raw = nightingaleRequestBody.TextBody;
                    break;
            }

            return result;
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
                    Item? request = ConvertRequest(item.Request);

                    if (request != null)
                    {
                        request.Name = item.Name;
                        list.Add(request);
                    }
                }
                else if (item.Items != null && item.Items.Length > 0)
                {
                    Item ngSubCollection = new Item
                    {
                        Type = ItemType.Collection,
                        Children = new List<Item>(),
                        Name = item.Name
                    };
                    IList<Item> ngChildren = ConvertItems(item.Items);
                    if (ngChildren != null)
                    {
                        ngSubCollection.Children.AddRange(ngChildren);
                    }

                    list.Add(ngSubCollection);
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
            result.SetProp(AuthConstants.BasicPassword,
                postmanAuth.Basic?.FirstOrDefault(x => x.Key == "password")?.Value);
            result.SetProp(AuthConstants.BasicUsername,
                postmanAuth.Basic?.FirstOrDefault(x => x.Key == "username")?.Value);

            // Oauth 1
            result.SetProp(AuthConstants.OAuth1ConsumerKey,
                postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "consumerKey")?.Value);
            result.SetProp(AuthConstants.OAuth1ConsumerSecret,
                postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "consumerSecret")?.Value);
            result.SetProp(AuthConstants.OAuth1AccessToken,
                postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "token")?.Value);
            result.SetProp(AuthConstants.OAuth1TokenSecret,
                postmanAuth.Oauth1?.FirstOrDefault(x => x.Key == "tokenSecret")?.Value);

            // Oauth 2
            result.SetProp(AuthConstants.OAuth2AccessToken,
                postmanAuth.Oauth2?.FirstOrDefault(x => x.Key == "accessToken")?.Value);

            // Bearer
            result.SetProp(AuthConstants.BearerToken, postmanAuth.Bearer?.FirstOrDefault(x => x.Key == "token")?.Value);

            // Digest
            result.SetProp(AuthConstants.DigestPassword,
                postmanAuth.Digest?.FirstOrDefault(x => x.Key == "password")?.Value);
            result.SetProp(AuthConstants.DigestUsername,
                postmanAuth.Digest?.FirstOrDefault(x => x.Key == "username")?.Value);


            if (Enum.TryParse(postmanAuth.Type, out PST.PostmanAuthType postmanAuthType)
                && Enum.IsDefined(typeof(AuthType), (int) postmanAuthType))
            {
                result.AuthType = (AuthType) postmanAuthType;
            }

            return result;
        }

        private Item? ConvertRequest(PST.Request postmanRequest)
        {
            if (postmanRequest == null)
            {
                return null;
            }

            var rawUrl = postmanRequest.Url?.Raw;
            var baseUrl = Uri.IsWellFormedUriString(rawUrl, UriKind.Absolute)
                ? new Uri(rawUrl).GetLeftPart(UriPartial.Path)
                : rawUrl;

            var result = new Item
            {
                Type = ItemType.Request,
                Url = new Url
                {
                    Base = baseUrl,
                    Queries = new List<Parameter>()
                },
                Body = ConvertBody(postmanRequest.Body),
                Method = postmanRequest.Method,
                Auth = ConvertAuth(postmanRequest.Auth),
                Headers = new List<Parameter>(),
                Properties = new Dictionary<string, object>(),
                MockData = new MockData(),
                Children = new List<Item>()
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
                        Type = ParamType.Parameter
                    });
                }
            }

            return result;
        }
    }
}