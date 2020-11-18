using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JeniusApps.Nightingale.Converters.Curl
{
    /// <summary>
    /// Class for converting curl statements to
    /// nightingale items.
    /// </summary>
    public class CurlConverter : ICurlConverter
    {
        /// <inheritdoc/>
        public Item Convert(string curlString)
        {
            if (string.IsNullOrWhiteSpace(curlString))
            {
                return null;
            }

            Item result = new()
            {
                Headers = new List<Parameter>(),
                Url = new Url(),
                Body = new RequestBody()
            };

            var args = ParseArguments(curlString);
            string data = "";

            for (int i = 0; i < args.Length; i++)
            {
                var current = args[i].ToLower();
                if (current == "curl")
                {
                    continue;
                }
                else if (current == "-x" || current == "--request")
                {
                    result.Method = args[i + 1].ToUpper();
                    i++;
                }
                else if (current == "-h" || current == "--header")
                {
                    var headerSplit = args[i + 1].Split(':');
                    var header = new Parameter
                    {
                        Enabled = true,
                        Key = headerSplit[0].Trim(),
                        Value = headerSplit[1].Trim(),
                        Type = ParamType.Header
                    };
                    result.Headers.Add(header);
                    i++;
                }
                else if (current == "-d" || current == "--data")
                {
                    data = args[i + 1];
                    i++;
                }
                else if (Uri.IsWellFormedUriString(current, UriKind.Absolute))
                {
                    result.Url.Base = args[i];
                }
            }

            if (!string.IsNullOrWhiteSpace(data))
            {
                // try to get the content type
                var contentType = result.Headers
                    .Where(x => x.Key.ToLower() == "content-type")
                    .FirstOrDefault()?.Value;

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    if (contentType.Contains("json"))
                    {
                        result.Body.BodyType = RequestBodyType.Json;
                        result.Body.JsonBody = data;
                    }
                    else if (contentType.Contains("xml"))
                    {
                        result.Body.BodyType = RequestBodyType.Xml;
                        result.Body.XmlBody = data;
                    }
                    else
                    {
                        result.Body.BodyType = RequestBodyType.Text;
                        result.Body.TextBody = data;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a command line string to an args array.
        /// </summary>
        public static string[] ParseArguments(string commandLine)
        {
            // Ref: https://stackoverflow.com/a/4140802
            string replacement = Regex.Replace(commandLine, @"\t|\\|\n|\r", "");

            // Ref: https://stackoverflow.com/a/298968
            char[] parmChars = replacement.ToCharArray();
            bool inSingleQuote = false;
            bool inDoubleQuote = false;
            for (int index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    parmChars[index] = '\n';
                }
                if (parmChars[index] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    parmChars[index] = '\n';
                }
                if (!inSingleQuote && !inDoubleQuote && char.IsWhiteSpace(parmChars[index]))
                {
                    parmChars[index] = '\n';
                }
            }
            return (new string(parmChars)).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
