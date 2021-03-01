using System.Linq;
using JeniusApps.Nightingale.Converters.Insomnia;
using JeniusApps.Nightingale.Data.Models;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace Nightingale.Test
{
    public class InsomniaConverterTests
    {
        [Fact]
        public void EnvironmentsAreImported()
        {
            void AssertEnvironmentVariables(Environment environment, string expectedTest, string expectedBaseUrl,
                string expectedEndpoint)
            {
                var testVariable = environment.Variables.FirstOrDefault(x => x.Key == "test");
                Assert.Equal(expectedTest, testVariable.Value);

                var baseUrlVariable = environment.Variables.FirstOrDefault(x => x.Key == "baseUrl");
                Assert.Equal(expectedBaseUrl, baseUrlVariable.Value);

                var endpointsVariable = environment.Variables.FirstOrDefault(x => x.Key == "endpoints.getItems");
                Assert.Equal(expectedEndpoint, endpointsVariable.Value);
            }

            var converter = new InsomniaConverterV4();

            var testJson = @"{
                ""_type"": ""export"",
                ""__export_format"": 4,
                ""__export_date"": ""2021-01-28T12:44:42.613Z"",
                ""__export_source"": ""insomnia.desktop.app:v2020.5.2"",
                ""resources"": [
                    {
                        ""_id"": ""wrk_75be171220c348b4b2f8ed4f3906b703"",
                        ""parentId"": null,
                        ""modified"": 1604067515742,
                        ""created"": 1604067515742,
                        ""name"": ""Test Workspace"",
                        ""scope"": null,
                        ""_type"": ""workspace""
                    },
                    {
                        ""_id"": ""env_b9d632b41e76095c43196a5c2e3b83a56bc5935d"",
                        ""parentId"": ""wrk_75be171220c348b4b2f8ed4f3906b703"",
                        ""modified"": 1604067515789,
                        ""created"": 1604067515789,
                        ""name"": ""Base Environment"",
                        ""data"": {
                            ""test"": ""variable""
                        },
                        ""metaSortKey"": 1604067515789,
                        ""_type"": ""environment""
                    },
                    {
                        ""_id"": ""env_a774483e752644918ab41a20ff33fbf4"",
                        ""parentId"": ""env_b9d632b41e76095c43196a5c2e3b83a56bc5935d"",
                        ""modified"": 1610045856735,
                        ""created"": 1604067689888,
                        ""name"": ""Local"",
                        ""data"": {
                            ""baseUrl"": ""https://localhost:5002/api"",
                            ""endpoints"": {
                                ""getItems"": ""getitems""
                            }
                        },
                        ""dataPropertyOrder"": {
                            ""&"": [
                                ""baseUrl"",
                                ""endpoints""
                            ],
                            ""&~|endpoints"": [
                                ""getitems""
                            ]
                        },
                        ""color"": ""#32d6ec"",
                        ""metaSortKey"": 0,
                        ""_type"": ""environment""
                    },
                    {
                        ""_id"": ""env_8887c691110143248e0913a90e6d0c9e"",
                        ""parentId"": ""env_b9d632b41e76095c43196a5c2e3b83a56bc5935d"",
                        ""modified"": 1610045802109,
                        ""created"": 1604067726637,
                        ""name"": ""Test"",
                        ""data"": {
                            ""baseUrl"": ""https://test.co.uk/api"",
                            ""endpoints"": {
                                ""getItems"": ""getitems""
                            }
                        },
                        ""dataPropertyOrder"": {
                            ""&"": [
                                ""baseUrl"",
                                ""endpoints""
                            ],
                            ""&~|endpoints"": [
                                ""getitems""
                            ]
                        },
                        ""color"": ""#f6e313"",
                        ""metaSortKey"": 1,
                        ""_type"": ""environment""
                    },
                    {
                        ""_id"": ""env_5415416d3c8345449dbf16b8e86ccf95"",
                        ""parentId"": ""env_b9d632b41e76095c43196a5c2e3b83a56bc5935d"",
                        ""modified"": 1610106982257,
                        ""created"": 1610045813748,
                        ""name"": ""Production"",
                        ""data"": {
                            ""baseUrl"": ""https://prod.co.uk/api"",
                            ""endpoints"": {
                                ""getItems"": ""getitems""
                            }
                        },
                        ""dataPropertyOrder"": {
                            ""&"": [
                                ""baseUrl"",
                                ""endpoints""
                            ],
                            ""&~|endpoints"": [
                                ""getitems""
                            ]
                        },
                        ""color"": ""#4413f6"",
                        ""metaSortKey"": 1610045813748,
                        ""_type"": ""environment""
                    },
                    {
                        ""_id"": ""jar_b9d632b41e76095c43196a5c2e3b83a56bc5935d"",
                        ""parentId"": ""wrk_75be171220c348b4b2f8ed4f3906b703"",
                        ""modified"": 1604067515792,
                        ""created"": 1604067515792,
                        ""name"": ""Default Jar"",
                        ""cookies"": [],
                        ""_type"": ""cookie_jar""
                    },
                    {
                        ""_id"": ""spc_d1b76187d9b543d9ae9ce82aac25c48c"",
                        ""parentId"": ""wrk_75be171220c348b4b2f8ed4f3906b703"",
                        ""modified"": 1604067515752,
                        ""created"": 1604067515752,
                        ""name"": ""Test Workspace"",
                        ""contents"": """",
                        ""contentType"": ""yaml"",
                        ""_type"": ""api_spec""
                    },
                    {
                        ""_id"": ""req_6d1ac4058ae545008c0ebceadf18f65c"",
                        ""parentId"": ""wrk_75be171220c348b4b2f8ed4f3906b703"",
                        ""modified"": 1611837700980,
                        ""created"": 1604507170353,
                        ""url"": ""{{ baseUrl }}/{{ endpoints.getItems }}"",
                        ""name"": ""GetItems"",
                        ""description"": """",
                        ""method"": ""GET"",
                        ""body"": {},
                        ""parameters"": [
                            {
                                ""name"": ""search"",
                                ""value"": ""todo"",
                                ""id"": ""pair_49de2f9089864b1791b9c6263a388b2d""
                            }
                        ],
                        ""headers"": [],
                        ""authentication"": {},
                        ""metaSortKey"": -1604507170353,
                        ""settingStoreCookies"": true,
                        ""settingSendCookies"": true,
                        ""settingEncodeUrl"": true,
                        ""settingRebuildPath"": true,
                        ""settingFollowRedirects"": ""global"",
                        ""_type"": ""request""
                    },
                ],
            }";

            var exportDoc = JsonConvert.DeserializeObject<Insomnia.NET.Models.ExportDoc>(testJson);

            var workspaces = converter.Convert(exportDoc);

            Assert.Equal(1, workspaces.Count);

            var workspace = workspaces.FirstOrDefault();

            Assert.Equal(3, workspace.Environments.Count);

            var localEnv = workspace.Environments.FirstOrDefault(x => x.Name == "Local");
            AssertEnvironmentVariables(localEnv, "variable", "https://localhost:5002/api", "getitems");

            var testEnv = workspace.Environments.FirstOrDefault(x => x.Name == "Test");
            AssertEnvironmentVariables(testEnv, "variable", "https://test.co.uk/api", "getitems");

            var prodEnv = workspace.Environments.FirstOrDefault(x => x.Name == "Production");
            AssertEnvironmentVariables(prodEnv, "variable", "https://prod.co.uk/api", "getitems");
        }
    }
}