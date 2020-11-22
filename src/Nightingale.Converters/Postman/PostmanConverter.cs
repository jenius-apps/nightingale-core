using JeniusApps.Nightingale.Data.Extensions;
using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static JeniusApps.Nightingale.Converters.Postman.AuthExtensions;
using PST = Postman.NET.Collections.Models;

namespace JeniusApps.Nightingale.Converters.Postman
{
    /// <summary>
    /// Class for converting Postman collections into Nightingale collections.
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

        /// <inheritdoc/>
        public PST.Collection? ConvertToCollection(Item item)
        {
            if (item == null) return null;
            PST.Collection pgCollection = new PST.Collection();
            if (item.Type == ItemType.Collection)
            {
                pgCollection.Info = new PST.Info {  Name = item.Name };
                var children = ConvertToItems(item.Children);
                if (children != null)
                {
                    pgCollection.Items = children;
                }
            }
            else if (item.Type == ItemType.Request)
            {
                // TODO: Maybe use something like "Exported from Nightinagle" instead? 
                pgCollection.Info = new PST.Info { Name = "Untitled Collection" };
                var pgItem = ConvertRequestToItem(item);
                if (pgItem == null)
                {
                    // Failed to ouput the only item. Return early.
                    // TODO: Logging? Microsoft.Extensions.Logging?
                    return null;
                }
                pgCollection.Items = new PST.Item[] { pgItem };
            }
            return pgCollection;
        }

        private PST.Item[]? ConvertToItems(List<Item> items)
        {
            var itemCount = items.Count;
            if (items == null || itemCount == 0) return new PST.Item[] { };
            var pgItems = new List<PST.Item>();
            foreach (var item in items)
            {
                if (item.Type == ItemType.Request)
                {
                    var request = ConvertRequestToItem(item);
                    if (request != null)
                    {
                        pgItems.Add(request);
                    }
                }
                else if (item.Type == ItemType.Collection)
                {
                    var pgItem = new PST.Item { Name = item.Name };
                    var children = ConvertToItems(item.Children);
                    if (children != null)
                    {
                        pgItem.Items = children;
                    }
                    pgItems.Add(pgItem);
                }
            }
            return pgItems.ToArray();
        }

        private PST.Item? ConvertRequestToItem(Item item)
        {
            if (item.Type != ItemType.Request) throw new ArgumentException($"ItemType must be Request, got {item.Type} instead.");
            var rawUrl = item.Url.ToString();
            var uri = Uri.IsWellFormedUriString(rawUrl, UriKind.Absolute)
                ? new Uri(rawUrl)
                : null;
            string? scheme = null;
            string? host = null;
            if (uri != null)
            {
                scheme = uri.Scheme;
                host = uri.Host;
            }
            var pgQueries = new List<PST.Query>();
            foreach (var param in item.Url.Queries)
            {
                pgQueries.Add(new PST.Query
                {
                    Disabled = !param.Enabled,
                    Key = param.Key,
                    Value = param.Value
                });
            }
            // TODO: Confirm what to do with Variable.
            var pgUrl = new PST.Url
            {
                Raw = rawUrl,
                Protocol = scheme,
                Host = host == null ? null : new string[] {host},
                Path = uri?.Segments,
                Query = pgQueries.ToArray(),
            };
            var pgHeaders = new List<PST.Parameter>();
            foreach (var header in item.Headers)
            {
                // TODO: Confirm what Type should be here.
                pgHeaders.Add(new PST.Parameter
                {
                    Key = header.Key,
                    Value = header.Value,
                    Disabled = !header.Enabled,
                    Type = "header"
                });
            }
            return new PST.Item
            {
                Name = item.Name,
                Request = new PST.Request
                {
                    Method = item.Method,
                    Auth = item.Auth == null ? null : ConvertToAuth(item.Auth),
                    Body = item.Body == null ? null : ConvertToBody(item.Body),
                    Url = pgUrl,
                    Header = pgHeaders.ToArray()
                }
            };
        }

        private PST.Body ConvertToBody(RequestBody body)
        {
            var pgBody = new PST.Body();
            if (body.BodyType == RequestBodyType.Json)
            {
                pgBody.Mode = "raw";
                pgBody.Options = new PST.BodyOptions
                {
                    Raw = new PST.RawOptions
                    {
                        Language = "json"
                    }
                };
            } 
            else if (body.BodyType == RequestBodyType.Xml)
            {
                pgBody.Mode = "raw";
                pgBody.Options = new PST.BodyOptions
                {
                    Raw = new PST.RawOptions
                    {
                        Language = "xml"
                    }
                };
            }
            else if (body.BodyType == RequestBodyType.Text)
            {
                pgBody.Mode = "raw";
            }
            else if (body.BodyType == RequestBodyType.FormEncoded && body.FormEncodedData != null)
            {
                pgBody.Mode = "urlencoded";
                var urlEncodedData = new List<PST.Parameter>();
                foreach (var datum in body.FormEncodedData)
                {
                    urlEncodedData.Add(new PST.Parameter
                    {
                        Key = datum.Key,
                        Value = datum.Value
                    });
                }
                pgBody.Urlencoded = urlEncodedData.ToArray();
            }
            else if (body.BodyType == RequestBodyType.FormData && body.FormDataList != null)
            {
                pgBody.Mode = "formdata";
                var formData = new List<PST.FormData>();
                foreach (var datum in body.FormDataList)
                {
                    var pgForm = new PST.FormData
                    {
                        ContentType = datum.ContentType,
                        Key = datum.Key,
                        Value = datum.Value,
                        Disabled = !datum.Enabled
                    };
                    if (datum.FormDataType == FormDataType.Text)
                    {
                        pgForm.Type = "text";
                    }
                    else if (datum.FormDataType == FormDataType.File)
                    {
                        pgForm.Type = "file";
                    }
                    var pgFiles = new List<string>();
                    foreach (var file in datum.FilePaths)
                    {
                        if (string.IsNullOrWhiteSpace(file))
                        {
                            continue;
                        }
                        pgFiles.Add(file.TrimStart('/'));
                    }
                    pgForm.Src = pgFiles;
                }
                pgBody.Formdata = formData.ToArray();
            }
            else if (body.BodyType == RequestBodyType.Binary)
            {
                pgBody.Mode = "file";
                pgBody.File = new PST.FileAttachment
                {
                    Src = body.BinaryFilePath.TrimStart('/')
                };
            }
            return pgBody;
        }

        private PST.Auth ConvertToAuth(Authentication auth)
        {
            var pgAuth = new PST.Auth
            {
                Basic = auth.ConvertToParameter(new AuthMapping[]
                {
                    new AuthMapping(AuthConstants.BasicPassword, "password"),
                    new AuthMapping(AuthConstants.BasicUsername, "username")
                }),
                Oauth1 = auth.ConvertToParameter(new AuthMapping[]
                {
                    new AuthMapping(AuthConstants.OAuth1ConsumerKey, "consumerKey"),
                    new AuthMapping(AuthConstants.OAuth1ConsumerSecret, "consumerSecret"),
                    new AuthMapping(AuthConstants.OAuth1AccessToken, "token"),
                    new AuthMapping(AuthConstants.OAuth1TokenSecret, "tokenSecret")
                }),
                Oauth2 = auth.ConvertToParameter(new AuthMapping[]
                {
                    new AuthMapping(AuthConstants.OAuth2AccessToken, "accessToken")
                }),
                Bearer = auth.ConvertToParameter(new AuthMapping[]
                {
                    new AuthMapping(AuthConstants.BearerToken, "token")
                }),
                Digest = auth.ConvertToParameter(new AuthMapping[]
                {
                    new AuthMapping(AuthConstants.DigestPassword, "password"),
                    new AuthMapping(AuthConstants.DigestUsername, "username")
                })
            };
            if (Enum.IsDefined(typeof(PST.PostmanAuthType), (int)auth.AuthType))
            {
                pgAuth.Type = ((PST.PostmanAuthType) auth.AuthType).ToString();
            }
            return pgAuth;
        }
    }

    internal static class AuthExtensions
    {
        internal class AuthMapping
        {
            internal AuthMapping(string name, string pgName)
            {
                Name = name;
                PgName = pgName;
            }
            public string Name { get; }
            public string PgName { get; }
        }

        internal static PST.Parameter[]? ConvertToParameter(this Authentication auth, AuthMapping[] propNames)
        {
            var props = new List<PST.Parameter>();
            foreach (var name in propNames)
            {
                var prop = auth.GetProp(name.Name);
                if (!string.IsNullOrWhiteSpace(prop))
                {
                    props.Add(new PST.Parameter
                    {
                        Key = name.PgName,
                        Value = prop
                    });
                }
            }
            return props.ToArray();
        }
    }
}
