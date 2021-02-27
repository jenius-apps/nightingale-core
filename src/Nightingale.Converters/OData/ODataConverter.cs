using JeniusApps.Nightingale.Data.Models;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JeniusApps.Nightingale.Converters.OData
{
    /// <summary>
    /// Class for converting a OData Metadata document to nightingale items
    /// </summary>
    public class ODataConverter : IODataConverter
    {
        /// <summary>
        /// That's the placesholder for all urls.
        /// </summary>
        public const string NameOfPlaceHolder = "baseurl";

        /// <inheritdoc/>
        public Item? ConvertCollection(IEdmModel edmModel)
        {
            if (edmModel == null)
            {
                return null;
            }
            var collection = new Item
            {
                Type = ItemType.Collection,
                Name = edmModel.EntityContainer.Name
            };
            collection.Children = GetAllEntities(edmModel.EntityContainer).ToList();
            return collection;
        }
        /// <inheritdoc/>
        public Item? ConvertCollection(string xml)
        {
            IEdmModel model;
            IEnumerable<EdmError> errors = new List<EdmError>();
            CsdlReader.TryParse(XmlReader.Create(new StringReader(xml)), out model, out errors);
            if(errors.Count() != 0)
            {
                throw new ODataException($"Can't convert the given metadata document. " +
                    $"There are {errors.Count()} erros in the document.");
            }
            return ConvertCollection(model);
        }

        private IEnumerable<Item> GetAllEntities(IEdmEntityContainer container)
        {

            foreach (var entity in container.Elements)
            {
                switch (entity.ContainerElementKind)
                {
                    case EdmContainerElementKind.EntitySet:
                        yield return GetEntityUrls((IEdmEntitySet)entity);
                        break;
                    case EdmContainerElementKind.ActionImport:
                        yield return GetActionUrls((IEdmActionImport)entity);
                        break;
                    case EdmContainerElementKind.FunctionImport:
                        yield return GetFunctionUrls((IEdmFunctionImport)entity);
                        break;
                    case EdmContainerElementKind.Singleton:
                        yield return GetSingeltoneUrls((IEdmSingleton)entity);
                        break;
                    default:
                        throw new NotImplementedException($"The type '{entity.ContainerElementKind}' " +
                            $"isn't implemented.");
                }
            }
        }
        private Item GetEntityUrls(IEdmEntitySet edmEntity)
        {
            var result = new Item
            {
                Type = ItemType.Collection,
                Name = edmEntity.Name
            };
            result.Children = new List<Item>()
            {
                new Item()
                {
                    Name = "List",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url() { Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}" }
                },
                new Item(){
                    Name = $"List full expanded",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url()
                    {
                        Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}",
                        Queries = new List<Parameter>() {
                                new Parameter() { Key = "$expand", Value = "*", Type = ParamType.Parameter }
                            }
                    }
                },
                new Item(){
                    Name = $"Single",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url() { Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}/{{{{id}}}}" }
                },
                new Item(){
                    Name = $"single full expanded",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url()
                    {
                        Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}/{{{{id}}}}",
                        Queries = new List<Parameter>() {
                                new Parameter() { Key = "$expand", Value = "*", Type = ParamType.Parameter }
                        }
                    }
                }
                // TODO: add support for bounded Actions
            };
            return result;
            
        }
        private Item GetSingeltoneUrls(IEdmSingleton edmEntity)
        {
            var result = new Item
            {
                Type = ItemType.Collection,
                Name = edmEntity.Name
            };
            result.Children = new List<Item>()
            {  
                new Item()
                {
                    Name = "single",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url() { Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}" }
                },
                new Item()
                {
                    Name = $"single full expanded",
                    Type = ItemType.Request,
                    Method = "GET",
                    Url = new Url()
                    {
                        Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}",
                        Queries = new List<Parameter>() {
                                new Parameter() { Key = "$expand", Value = "*", Type = ParamType.Parameter }
                            }
                    }
                }
                // TODO: add support for bounded Actions
            };
            return result;
        }
        private Item GetFunctionUrls(IEdmFunctionImport edmEntity)
        {
            var functionParameters = string.Join(",", edmEntity.Function.Parameters.Select(d =>
             {
                 return $"{d.Name} = {{{{{d.Name}}}}}";
             }));
            return new Item()
            {
                Name = edmEntity.Name,
                Type = ItemType.Request,
                Method = "GET",
                Url = new Url()
                {
                    Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}({functionParameters})"
                }
            };
        }
        private Item GetActionUrls(IEdmActionImport edmEntity)
        {
            var functionParameters = string.Join(",", edmEntity.Action.Parameters.Select(d =>
            {
                return $"{d.Name} = {{{{{d.Name}}}}}";
            }));
            return new Item()
            {
                Name = edmEntity.Name,
                Type = ItemType.Request,
                Method = "GET",
                Url = new Url()
                {
                    Base = $"{{{{{NameOfPlaceHolder}}}}}/{edmEntity.Name}({functionParameters})"
                }
            };
        }
    }
}
