using JeniusApps.Nightingale.Data.Models;
using Newtonsoft.Json;
using JeniusApps.Nightingale.Converters.Postman;
using Postman.NET.Collections.Models;
using System;
using System.Linq;
using Xunit;
using Item = JeniusApps.Nightingale.Data.Models.Item;

namespace Nightingale.Test
{
    public class PostmanConverterTest
    {
        [Fact]
        public void FolderIsRetained()
        {
            var converter = new PostmanConverter();

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
            var ngResult = converter.ConvertCollection(ptCollections);
            var firstNgItem = ngResult.Children.First();
            Assert.True(firstNgItem.Name == "Test folder");
            Assert.True(firstNgItem.Children.Count == 1);
        }

        [Fact]
        public void QueryParameterIsTheSame()
        {
            var converter = new PostmanConverter();
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
            var ngResult = converter.ConvertCollection(ptCollections);
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
            var converter = new PostmanConverter();
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
            var ngResult = converter.ConvertCollection(ptCollections);
            Assert.NotNull(ngResult.Children[0].Body.FormDataList);
            Assert.Equal("asdf", ngResult.Children[0].Body.FormDataList[0].Key);
            Assert.Equal(FormDataType.File, ngResult.Children[0].Body.FormDataList[0].FormDataType);
        }

        [Fact]
        public void NoRedundantQueries()
        {
            var converter = new PostmanConverter();
            var testJson = @"{
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
				""protocolProfileBehavior"": {}
			}";

            var ptcollection = JsonConvert.DeserializeObject<Collection>(testJson);
            var ngItem = converter.ConvertCollection(ptcollection);
            Assert.Equal("http://localhost:3001/", ngItem.Children.First().Url.Base);
        }

        /// <summary>
        /// Test to make sure no exceptions
        /// occur when adding headers.
        /// </summary>
        [Fact]
        public void AddHeadersTest()
        {
            var converter = new PostmanConverter();
            var testJson = @"{
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
							""header"": [
								{
									""key"": ""Content-Type"",
									""name"": ""Content-Type"",
									""value"": ""application/json"",
									""type"": ""text""
								}
							],
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
				""protocolProfileBehavior"": {}
			}";

            var ptcollection = JsonConvert.DeserializeObject<Collection>(testJson);
            var ngItem = converter.ConvertCollection(ptcollection);
            Assert.Equal("application/json", ngItem.Children[0].Headers[0].Value);
        }

        [Fact]
        public void NgToPostman_QueryIsTranslated()
        {
            var convert = new PostmanConverter();
            var data = @"
				{
			    ""Id"": ""292842f1-009d-485a-b0dc-13c4218f47f8"",
			    ""IsTemporary"": false,
			    ""Properties"": {},
			    ""Url"": {
			      ""Base"": null,
			      ""Queries"": []
			    },
			    ""Auth"": {
			      ""Id"": null,
			      ""ParentId"": null,
			      ""AuthType"": 0,
			      ""OAuth2GrantType"": 0,
			      ""AuthProperties"": null
			    },
			    ""Body"": {
			      ""Id"": null,
			      ""ParentId"": null,
			      ""BodyType"": 0,
			      ""FormEncodedData"": [],
			      ""FormDataList"": []
			    },
			    ""MockData"": {
			      ""Body"": null,
			      ""StatusCode"": 200,
			      ""ContentType"": ""application/json""
			    },
			    ""Children"": [
			      {
			        ""Id"": ""b9349c45-4e2c-40f6-a5df-d5eb40bba354"",
			        ""IsTemporary"": false,
			        ""Properties"": {},
			        ""Url"": {
			          ""Base"": ""https://test.com?"",
			          ""Queries"": [
			            {
			              ""Id"": null,
			              ""ParentId"": null,
			              ""Key"": ""testquery"",
			              ""Value"": ""testqueryvalue"",
			              ""Enabled"": true,
			              ""Private"": false,
			              ""Type"": 0
			            }
			          ]
			        },
			        ""Auth"": {
			          ""Id"": null,
			          ""ParentId"": null,
			          ""AuthType"": 0,
			          ""OAuth2GrantType"": 0,
			          ""AuthProperties"": null
			        },
			        ""Body"": {
			          ""Id"": null,
			          ""ParentId"": null,
			          ""BodyType"": 0,
			          ""FormEncodedData"": [],
			          ""FormDataList"": []
			        },
			        ""MockData"": {
			          ""Body"": null,
			          ""StatusCode"": 200,
			          ""ContentType"": ""application/json""
			        },
			        ""Children"": [],
			        ""Headers"": [],
			        ""ChainingRules"": [],
			        ""Type"": 1,
			        ""Name"": ""test query request"",
			        ""IsExpanded"": false,
			        ""Method"": ""GET"",
			        ""Response"": null
			      }
			    ],
			    ""Headers"": [],
			    ""ChainingRules"": [],
			    ""Type"": 2,
			    ""Name"": ""ConvertTest"",
			    ""IsExpanded"": true,
			    ""Method"": ""GET"",
			    ""Response"": null
			  }
			";
            var ngItem = JsonConvert.DeserializeObject<Item>(data);
            var ptCollection = convert.ConvertCollection(ngItem);

            Assert.Single(ptCollection.Items);
            var item = ptCollection.Items[0];

            Assert.Equal("test query request", item.Name);
            Assert.Equal("testquery", item.Request.Url.Query[0].Key);
            Assert.Equal("GET", item.Request.Method);
            Assert.Equal("testqueryvalue", item.Request.Url.Query[0].Value);
        }

        [Fact]
        public void NgToPostman_FormDataIsTranslated()
        {
            var converter = new PostmanConverter();
            var data = @"
				{
		    ""Id"": ""292842f1-009d-485a-b0dc-13c4218f47f8"",
		    ""IsTemporary"": false,
		    ""Properties"": {},
		    ""Url"": {
		      ""Base"": null,
		      ""Queries"": []
		    },
		    ""Auth"": {
		      ""Id"": null,
		      ""ParentId"": null,
		      ""AuthType"": 0,
		      ""OAuth2GrantType"": 0,
		      ""AuthProperties"": null
		    },
		    ""Body"": {
		      ""Id"": null,
		      ""ParentId"": null,
		      ""BodyType"": 0,
		      ""FormEncodedData"": [],
		      ""FormDataList"": []
		    },
		    ""MockData"": {
		      ""Body"": null,
		      ""StatusCode"": 200,
		      ""ContentType"": ""application/json""
		    },
		    ""Children"": [
		      {
		        ""Id"": ""523c7c50-4b22-4d14-9385-d2b096f86dd0"",
		        ""IsTemporary"": false,
		        ""Properties"": {
		          ""RequestPivotIndex"": 3
		        },
		        ""Url"": {
		          ""Base"": """",
		          ""Queries"": []
		        },
		        ""Auth"": {
		          ""Id"": null,
		          ""ParentId"": null,
		          ""AuthType"": 0,
		          ""OAuth2GrantType"": 0,
		          ""AuthProperties"": null
		        },
		        ""Body"": {
		          ""Id"": null,
		          ""ParentId"": null,
		          ""BodyType"": 5,
		          ""FormEncodedData"": [],
		          ""FormDataList"": [
		            {
		              ""Id"": null,
		              ""ParentId"": null,
		              ""Key"": ""formdatatest"",
		              ""Value"": ""formadatavalue"",
		              ""Enabled"": true,
		              ""FormDataType"": 0,
		              ""FilePaths"": null
		            }
		          ]
		        },
		        ""MockData"": {
		          ""Body"": null,
		          ""StatusCode"": 200,
		          ""ContentType"": ""application/json""
		        },
		        ""Children"": [],
		        ""Headers"": [],
		        ""ChainingRules"": [],
		        ""Type"": 1,
		        ""Name"": ""Test form data request"",
		        ""IsExpanded"": false,
		        ""Method"": ""POST"",
		        ""Response"": null
		      }
		    ],
		    ""Headers"": [],
		    ""ChainingRules"": [],
		    ""Type"": 2,
		    ""Name"": ""ConvertTest"",
		    ""IsExpanded"": true,
		    ""Method"": ""GET"",
		    ""Response"": null
		  }
		";

            var ngItem = JsonConvert.DeserializeObject<Item>(data);
            var ptCollection = converter.ConvertCollection(ngItem);

            Assert.Single(ptCollection.Items);
            var item = ptCollection.Items[0];

            Assert.Equal("Test form data request", item.Name);
            Assert.Single(item.Request.Body.Formdata);
            Assert.Equal("formdatatest", item.Request.Body.Formdata[0].Key);
            Assert.Equal("POST", item.Request.Method);
            Assert.Equal("formadatavalue", item.Request.Body.Formdata[0].Value);
        }

        [Fact]
        public void NgToPostman_HeadersIsTranslated()
        {
            var converter = new PostmanConverter();
            var data = @"
			{
          ""Id"": ""292842f1-009d-485a-b0dc-13c4218f47f8"",
          ""IsTemporary"": false,
          ""Properties"": {},
          ""Url"": {
            ""Base"": null,
            ""Queries"": []
          },
          ""Auth"": {
            ""Id"": null,
            ""ParentId"": null,
            ""AuthType"": 0,
            ""OAuth2GrantType"": 0,
            ""AuthProperties"": null
          },
          ""Body"": {
            ""Id"": null,
            ""ParentId"": null,
            ""BodyType"": 0,
            ""FormEncodedData"": [],
            ""FormDataList"": []
          },
          ""MockData"": {
            ""Body"": null,
            ""StatusCode"": 200,
            ""ContentType"": ""application/json""
          },
          ""Children"": [
            {
              ""Id"": ""9f2f6ecb-fa29-4e79-bf4d-97eb9723a189"",
              ""IsTemporary"": false,
              ""Properties"": {
                ""RequestPivotIndex"": 2
              },
              ""Url"": {
                ""Base"": ""https://test.com"",
                ""Queries"": []
              },
              ""Auth"": {
                ""Id"": null,
                ""ParentId"": null,
                ""AuthType"": 0,
                ""OAuth2GrantType"": 0,
                ""AuthProperties"": null
              },
              ""Body"": {
                ""Id"": null,
                ""ParentId"": null,
                ""BodyType"": 0,
                ""FormEncodedData"": [],
                ""FormDataList"": [
                  {
                    ""Id"": null,
                    ""ParentId"": null,
                    ""Enabled"": true,
                    ""FormDataType"": 0,
                    ""FilePaths"": null
                  }
                ]
              },
              ""MockData"": {
                ""Body"": null,
                ""StatusCode"": 200,
                ""ContentType"": ""application/json""
              },
              ""Children"": [],
              ""Headers"": [
                {
                  ""Id"": null,
                  ""ParentId"": null,
                  ""Key"": ""testheader"",
                  ""Value"": ""testheadervalue"",
                  ""Enabled"": true,
                  ""Private"": false,
                  ""Type"": 1
                }
              ],
              ""ChainingRules"": [],
              ""Type"": 1,
              ""Name"": ""Test Header Request"",
              ""IsExpanded"": false,
              ""Method"": ""POST"",
              ""Response"": null
            }
          ],
          ""Headers"": [],
          ""ChainingRules"": [],
          ""Type"": 2,
          ""Name"": ""ConvertTest"",
          ""IsExpanded"": true,
          ""Method"": ""GET"",
          ""Response"": null
        }
		";

            var ngItem = JsonConvert.DeserializeObject<Item>(data);
            var ptCollection = converter.ConvertCollection(ngItem);

            Assert.Single(ptCollection.Items);
            var item = ptCollection.Items[0];

            Assert.Equal("Test Header Request", item.Name);
            Assert.Single(item.Request.Header);
            Assert.Equal("testheader", item.Request.Header[0].Key);
            Assert.Equal("POST", item.Request.Method);
            Assert.Equal("testheadervalue", item.Request.Header[0].Value);
        }
    }
}

