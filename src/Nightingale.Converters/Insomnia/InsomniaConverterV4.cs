using INS = Insomnia.NET.Models;
using JeniusApps.Nightingale.Data.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using JeniusApps.Nightingale.Data.Extensions;
using MimeMapping;

namespace JeniusApps.Nightingale.Converters.Insomnia
{
    /// <summary>
    /// Class for converting Insomnia Export v4 files.
    /// </summary>
    public class InsomniaConverterV4 : IInsomniaConverter
    {
        /// <inheritdoc/>
        public IList<Workspace> Convert(INS.ExportDoc exportFile)
        {
            if (exportFile == null)
            {
                return null;
            }

            List<Workspace> result = new List<Workspace>();

            // Retrieve all workspaces
            var workspaces = exportFile.Resources
                .Where(x => x._type == "workspace")
                .ToList();

            foreach (var workspace in workspaces)
            {
                var nglWorkspace = new Workspace
                {
                    Name = workspace.name
                };

                var workspaceItems = exportFile.Resources
                    .Where(x => x.parentId == workspace._id)
                    .ToList();

                foreach (var item in workspaceItems)
                {
                    // TODO transfer cookies

                    if (item._type == "environment")
                    {
                        TransferEnv(item, nglWorkspace.Environments);
                        continue;
                    }

                    Item workspaceItem = ConvertWorkspaceItem(item, exportFile.Resources);
                    if (workspaceItem != null)
                    {
                        nglWorkspace.Items.Add(workspaceItem);
                    }
                }

                result.Add(nglWorkspace);
            }

            return result;
        }

        private void TransferEnv(
            dynamic env,
            IList<Environment> to)
        {
            if (env is JToken token)
            {
                var insomniaEnv = token.ToObject<INS.Environment>();

                var ntgEnv = new Environment
                {
                    Name = insomniaEnv.Name
                };

                foreach (var pair in insomniaEnv.Data)
                {
                    ntgEnv.Variables.Add(new Parameter
                    {
                        Key = pair.Key,
                        Value = pair.Value,
                        Enabled = true
                    });
                }

                to.Add(ntgEnv);
            }
        }

        private Authentication ConvertAuth(INS.Authentication auth)
        {
            if (auth == null)
            {
                return null;
            }

            var result = new Authentication();
            result.SetProp(AuthConstants.BasicUsername, auth.Username);
            result.SetProp(AuthConstants.BasicPassword, auth.Password);
            return result;
        }

        private void TransferParameter(
            IList<INS.BodyParameter> from,
            IList<FormData> to)
        {
            if (from == null || to == null)
            {
                return;
            }

            foreach (var insomniaItem in from)
            {
                var ntgItem = new FormData
                {
                    Key = insomniaItem.Name,
                    Value = insomniaItem.Value,
                    FilePaths = !string.IsNullOrWhiteSpace(insomniaItem.FileName)
                        ? new List<string> { insomniaItem.FileName }
                        : null,
                    FormDataType = insomniaItem.Type == "file" ? FormDataType.File : FormDataType.Text,
                    Enabled = !insomniaItem.Disabled
                };

                to.Add(ntgItem);
            }
        }

        private void TransferParameter(
            IList<INS.BodyParameter> from,
            IList<Parameter> to,
            ParamType paramType)
        {
            if (from == null || to == null)
            {
                return;
            }

            foreach (var insomniaItem in from)
            {
                var ntgItem = new Parameter
                {
                    Key = insomniaItem.Name,
                    Value = insomniaItem.Value,
                    Type = paramType,
                    Enabled = !insomniaItem.Disabled
                };

                to.Add(ntgItem);
            }
        }

        private void TransferParameter(
            IList<INS.Parameter> from,
            IList<Parameter> to,
            ParamType paramType)
        {
            if (from == null || to == null)
            {
                return;
            }

            foreach (var insomniaItem in from)
            {
                var ntgItem = new Parameter
                {
                    Key = insomniaItem.Name,
                    Value = insomniaItem.Value,
                    Type = paramType,
                    Enabled = !insomniaItem.Disabled
                };

                to.Add(ntgItem);
            }
        }

        private RequestBody ConvertBody(INS.Body insomniaBody)
        {
            if (insomniaBody == null)
            {
                return null;
            }

            var ntgBody = new RequestBody();

            switch (insomniaBody.MimeType)
            {
                case KnownMimeTypes.Json:
                    ntgBody.JsonBody = insomniaBody.Text;
                    ntgBody.BodyType = RequestBodyType.Json;
                    break;
                case KnownMimeTypes.Xml:
                    ntgBody.XmlBody = insomniaBody.Text;
                    ntgBody.BodyType = RequestBodyType.Xml;
                    break;
                case KnownMimeTypes.Text:
                    ntgBody.TextBody = insomniaBody.Text;
                    ntgBody.BodyType = RequestBodyType.Text;
                    break;
                case "multipart/form-data":
                    TransferParameter(insomniaBody.Parameters, ntgBody.FormDataList);
                    ntgBody.BodyType = RequestBodyType.FormData;
                    break;
                case "application/x-www-form-urlencoded":
                    TransferParameter(insomniaBody.Parameters, ntgBody.FormEncodedData, ParamType.FormEncodedData);
                    ntgBody.BodyType = RequestBodyType.FormEncoded;
                    break;
                case KnownMimeTypes.Bin:
                    ntgBody.BinaryFilePath = insomniaBody.FileName;
                    break;
                default:
                    break;
            }

            return ntgBody;
        }

        private Item ConvertRequest(dynamic request)
        {
            if (request == null
                || request._type != "request")
            {
                return null;
            }

            if (request is JToken token)
            {
                var insomniaRequest = token.ToObject<INS.Request>();

                var workspaceRequest = new Item
                {
                    Name = insomniaRequest.Name,
                    Method = insomniaRequest.Method,
                    Type = ItemType.Request,
                    Url = new Url()
                    {
                        Base = insomniaRequest.Url
                    },
                    Auth = ConvertAuth(insomniaRequest.Authentication) ?? new Authentication(),
                    Body = ConvertBody(insomniaRequest.Body) ?? new RequestBody()
                };

                // Transfer headers
                TransferParameter(insomniaRequest.Headers, workspaceRequest.Headers, ParamType.Header);

                // Transfer queries
                TransferParameter(insomniaRequest.Parameters, workspaceRequest.Url.Queries, ParamType.Parameter);

                return workspaceRequest;
            }

            return null;
        }

        private Item ConvertWorkspaceItem(dynamic workspaceItem, List<dynamic> resourceList)
        {
            if (workspaceItem == null)
            {
                return null;
            }

            if (workspaceItem._type == "request_group")
            {
                return ConvertCollection(workspaceItem, resourceList);
            }
            else if (workspaceItem._type == "request")
            {
                return ConvertRequest(workspaceItem);
            }
            else
            {
                return null;
            }
        }

        private Item ConvertCollection(dynamic parentCollection, List<dynamic> resourceList)
        {
            if (parentCollection == null
                || resourceList == null
                || resourceList.Count == 0
                || (parentCollection._id is string s && string.IsNullOrWhiteSpace(s))
                || parentCollection._type != "request_group")
            {
                return null;
            }

            var workspaceCollection = new Item
            {
                Type = ItemType.Collection,
                Name = (string)parentCollection.name
            };

            var children = resourceList
                .Where(child => child.parentId == parentCollection._id)
                .ToList();

            if (children == null || children.Count == 0)
            {
                return workspaceCollection;
            }

            foreach (var child in children)
            {
                Item childItem = ConvertWorkspaceItem(child, resourceList);
                if (childItem != null)
                {
                    workspaceCollection.Children.Add(childItem);
                }
            }

            return workspaceCollection;
        }
    }
}
