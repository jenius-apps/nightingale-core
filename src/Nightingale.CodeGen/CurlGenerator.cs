using JeniusApps.Nightingale.Data.Extensions;
using JeniusApps.Nightingale.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace JeniusApps.Nightingale.CodeGen
{
    /// <summary>
    /// Translates Nightingale data into curl requests.
    /// </summary>
    public class CurlGenerator : ICodeGenerator
    {
        private readonly string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        /// <inheritdoc/>
        public string OutputName { get; } = "curl";

        /// <inheritdoc/>
        public string GenerateCode(Item item)
        {
            var commandsList = new List<string>();

            // Initialize
            commandsList.Add($"curl -X {item.Method}");

            // URL
            commandsList.Add($"\t\'{item.Url}\'");

            // headers
            foreach (Parameter p in item.Headers.GetActive())
            {
                commandsList.Add($"\t-H \'{p.Key}\': \'{p.Value}\'");
            }

            // body
            if (item.Body.BodyType == RequestBodyType.Json)
            {
                commandsList.Add($"\t-d \'{JsonConvert.SerializeObject(item.Body.JsonBody)}\'");
            }
            else if (item.Body.BodyType == RequestBodyType.Xml)
            {
                string xmlBodyString = string.Empty;

                try
                {
                    XDocument doc = XDocument.Parse(StripBom(item.Body.XmlBody));
                    xmlBodyString = $"@\"{doc.ToString().Replace('"', '\'')}\"";
                }
                catch { }

                commandsList.Add($"\t-d \'{xmlBodyString}\'");
            }
            else if (item.Body.BodyType == RequestBodyType.FormEncoded)
            {
                var pairs = new List<string>();
                foreach (Parameter p in item.Body.FormEncodedData.GetActive())
                {
                    pairs.Add($"{p.Key}={Uri.EscapeUriString(p.Value.ToString())}");
                }

                commandsList.Add($"\t-d \'{string.Join("&", pairs)}\'");
            }
            else if (item.Body.BodyType == RequestBodyType.Binary)
            {
                commandsList.Add($"\t-d \'@{item.Body.BinaryFilePath}\'");
            }

            return string.Join(" \\" + System.Environment.NewLine, commandsList);
        }

        private string StripBom(string xmlInput)
        {
            if (xmlInput.StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
            {
                xmlInput = xmlInput.Remove(0, _byteOrderMarkUtf8.Length);
            }

            return xmlInput.Trim();
        }
    }
}
