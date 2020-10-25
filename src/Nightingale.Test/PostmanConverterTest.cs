using JeniusApps.Nightingale.Data.Models;
using Newtonsoft.Json;
using JeniusApps.Nightingale.Converters.Postman;
using Postman.NET.Collections.Models;
using System;
using System.Linq;
using Xunit;

namespace Nightingale.Test
{
    public class PostmanConverterTest
    {
		private PostmanConverter _postmanConverter;
		public PostmanConverterTest()
        {
			_postmanConverter = new PostmanConverter();
		}
        [Fact]
        public void FolderIsRetained()
        {
            var folderJson = @"{
				""info"": {
					""_postman_id"": ""d3232d7e-a773-4953-8b80-0b5aa3fd79a5"",
					""name"": ""Test Collection"",
					""schema"": ""https://schema.getpostman.com/json/collection/v2.1.0/collection.json""
				},
				""item"": [
					{
						""name"": ""Test folder"",
						""item"": [
							{
								""name"": ""Test request"",
								""request"": {
									""method"": ""GET"",
									""header"": [],
									""url"": {
										""raw"": ""http://localhost:3001?param1=a&param2=b"",
										""protocol"": ""http"",
										""host"": [
											""localhost""
										],
										""port"": ""3001"",
										""query"": [
											{
												""key"": ""param1"",
												""value"": ""a""
											},
											{
												""key"": ""param2"",
												""value"": ""b""
											}
										]
									}
								},
								""response"": []
							}
						],
						""protocolProfileBehavior"": { }
					}
				],
				""protocolProfileBehavior"": { }
			}";
			var ptCollections = JsonConvert.DeserializeObject<Collection>(folderJson);
			var ngResult = _postmanConverter.ConvertCollection(ptCollections);
			var firstNgItem = ngResult.Children.First();
            Assert.True(firstNgItem.Name == "Test folder");
			Assert.True(firstNgItem.Children.Count == 1);
        }
		[Fact]
		public void QueryParameterIsTheSame()
		{
			var folderJson = @"{
				""info"": {
					""_postman_id"": ""d3232d7e-a773-4953-8b80-0b5aa3fd79a5"",
					""name"": ""Test Collection"",
					""schema"": ""https://schema.getpostman.com/json/collection/v2.1.0/collection.json""
				},
				""item"": [
					{
						""name"": ""Test request"",
						""request"": {
							""method"": ""GET"",
							""header"": [],
							""url"": {
								""raw"": ""http://localhost:3001?param1=a&param2=b"",
								""protocol"": ""http"",
								""host"": [
									""localhost""
								],
								""port"": ""3001"",
								""query"": [
									{
										""key"": ""param1"",
										""value"": ""a""
									},
									{
										""key"": ""param2"",
										""value"": ""b""
									}
								]
							}
						},
						""response"": []
					}
				],
				""protocolProfileBehavior"": { }
			}";
			var ptCollections = JsonConvert.DeserializeObject<Collection>(folderJson);
			var ngResult = _postmanConverter.ConvertCollection(ptCollections);
			var ptRequest = ptCollections.Items.First();
			var ngRequest = ngResult.Children.First();
			Assert.True(ngRequest.Type == ItemType.Request);
			var ptQuery = ptRequest.Request.Url.Query;
			var ngQueries = ngRequest.Url.Queries;
			Assert.True(ngQueries.Count == ptQuery.Count());
			var count = ngQueries.Count;
			for (var i = 0; i < count; ++i)
            {
				Assert.Equal(ParamType.Parameter, ngQueries[i].Type);
				Assert.Equal(ptQuery.ElementAt(i).Key, ngQueries[i].Key);
				Assert.Equal(ptQuery.ElementAt(i).Value, ngQueries[i].Value);
            }
		}

		[Fact]
		public void FormDataIsSuccessfullyImported()
        {
			var testJson = @"{
				""info"": {
					""_postman_id"": ""4f95d40b-14e5-4a11-ae65-2f90190b9836"",
					""name"": ""2552"",
					""schema"": ""https://schema.getpostman.com/json/collection/v2.1.0/collection.json""
				},
				""item"": [
					{
						""name"": ""555"",
						""protocolProfileBehavior"": {
							""disableBodyPruning"": true
						},
						""request"": {
							""method"": ""GET"",
							""header"": [],
							""body"": {
								""mode"": ""formdata"",
								""formdata"": [
									{
										""key"": ""asdf"",
										""contentType"": ""asdf"",
										""description"": ""asdf"",
										""type"": ""file"",
										""src"": ""/C:/Users/kid_j/Downloads/2552.postman_collection.json""
									}
								]
							},
							""url"": {
								""raw"": ""asdfasdf"",
								""host"": [
									""asdfasdf""
								]
							}
						},
						""response"": []
					}
				],
				""protocolProfileBehavior"": {}
			}";

			var ptCollections = JsonConvert.DeserializeObject<Collection>(testJson);

			// ensure no exception
			var ngResult = _postmanConverter.ConvertCollection(ptCollections);
			Assert.NotNull(ngResult.Children[0].Body.FormDataList);
			Assert.Equal("asdf", ngResult.Children[0].Body.FormDataList[0].Key);
			Assert.Equal(FormDataType.File, ngResult.Children[0].Body.FormDataList[0].FormDataType);
		}
	}
}
