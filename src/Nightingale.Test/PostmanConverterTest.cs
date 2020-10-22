using Newtonsoft.Json;
using Nightingale.Converters.Postman;
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
			var postmanCollections = JsonConvert.DeserializeObject<Collection>(folderJson);
			var nightingaleResult = _postmanConverter.ConvertCollection(postmanCollections);
			var firstNightingaleItem = nightingaleResult.Children.First();
            Assert.True(firstNightingaleItem.Name == "Test folder");
			Assert.True(firstNightingaleItem.Children.Count == 1);
        }
    }
}
