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
	}
}
