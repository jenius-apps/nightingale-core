using JeniusApps.Nightingale.Converters.Curl;
using JeniusApps.Nightingale.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Nightingale.Test
{
    public class CurlConverterTests
    {
        [Fact]
        public void CommandLineParseTest()
        {
            var input = @"curl 'https://appcenter.ms/api/v0.1/apps/jenius_apps/Ambie/analytics/languages/?start=2020-10-18T06%3A08%3A04.735Z&$top=7' \
-H 'authority: appcenter.ms' \
-H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36 Edg/86.0.622.69' \
-H 'diagnostic-context: 42ff43c8-a579-4e1d-8465-f8e644384f73' \
-H 'internal-request-source: portal' \
-H 'authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJLFaamoFOWPjGf54Jdy26JCtztq8RRGOgKIP6IwLiYjPb6Dg' \
-H 'accept: */*' \
-H 'sec-fetch-site: same-origin' \
-H 'sec-fetch-mode: cors' \
-H 'sec-fetch-dest: empty' \
-H 'accept-language: en-US,en;q=0.9' \
-H 'cookie: MSCC=1596513642; ai_user=J/3y0|2020-08-04T04:00:42.856Z; ajs_group_id=null; ajs_anonymous_id=%22c48c3860-7a21-4b2b-a723-0f49ca43a24c%22; _csrf=0Avm4Guu1Avd5PENLy5u5Vvf; ai_session=gQj0+|1605595865826|1605596884272.81; session=eyJ0aW1lc3RhbXAiOiIyMDIwLTExLTE3VDA3OjA4OjA0LjM0NFoiLCJmbGFzaCI6e30sImxvZ2luLXNvdXJjZSI6eyJ0eXBlIjoiYWFkIiwiaXNNaWNyb3NvZnRUZW5hbnQiOmZhbHNlfSwidXNlci1vcmlnaW4iOiJhcHBjZW50ZXIiLCJrbXNpIjp0cnVlLCJ0aWQiOiJmOGNkZWYzMS1hMzFlLTRiNGEtOTNlNC01ZjU3MWU5MTI1NWEiLCJwYXNzcG9ydCI6eyJ1c2VyIjoie1wiaWRcIjpcIjdmZjUwYWU5LWEyOWEtNDliOC05MTVjLTZhMzQyNGNmZTMwM1wifSJ9LCJzZXNzaW9uLWhhc2giOm51bGx9; session.sig=Sa6U_EdayPjfgumdUd3BtfvDCpA' \
--compressed";

            var args = CurlConverter.ParseArguments(input);
            Assert.True(args.Length == 25);
            Assert.Equal("https://appcenter.ms/api/v0.1/apps/jenius_apps/Ambie/analytics/languages/?start=2020-10-18T06%3A08%3A04.735Z&$top=7", args[1]);
        }

        [Fact]
        public void HeaderTest()
        {
            var input = @"curl 'https://appcenter.ms/api/v0.1/apps/jenius_apps/Ambie/analytics/languages/?start=2020-10-18T06%3A08%3A04.735Z&$top=7' \
-H 'authority: appcenter.ms' \
-H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36 Edg/86.0.622.69' \
-H 'diagnostic-context: 42ff43c8-a579-4e1d-8465-f8e644384f73' \
-H 'internal-request-source: portal' \
-H 'authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJLFaamoFOWPjGf54Jdy26JCtztq8RRGOgKIP6IwLiYjPb6Dg' \
-H 'accept: */*' \
-H 'sec-fetch-site: same-origin' \
-H 'sec-fetch-mode: cors' \
-H 'sec-fetch-dest: empty' \
-H 'accept-language: en-US,en;q=0.9' \
-H 'cookie: MSCC=1596513642; ai_user=J/3y0|2020-08-04T04:00:42.856Z; ajs_group_id=null; ajs_anonymous_id=%22c48c3860-7a21-4b2b-a723-0f49ca43a24c%22; _csrf=0Avm4Guu1Avd5PENLy5u5Vvf; ai_session=gQj0+|1605595865826|1605596884272.81; session=eyJ0aW1lc3RhbXAiOiIyMDIwLTExLTE3VDA3OjA4OjA0LjM0NFoiLCJmbGFzaCI6e30sImxvZ2luLXNvdXJjZSI6eyJ0eXBlIjoiYWFkIiwiaXNNaWNyb3NvZnRUZW5hbnQiOmZhbHNlfSwidXNlci1vcmlnaW4iOiJhcHBjZW50ZXIiLCJrbXNpIjp0cnVlLCJ0aWQiOiJmOGNkZWYzMS1hMzFlLTRiNGEtOTNlNC01ZjU3MWU5MTI1NWEiLCJwYXNzcG9ydCI6eyJ1c2VyIjoie1wiaWRcIjpcIjdmZjUwYWU5LWEyOWEtNDliOC05MTVjLTZhMzQyNGNmZTMwM1wifSJ9LCJzZXNzaW9uLWhhc2giOm51bGx9; session.sig=Sa6U_EdayPjfgumdUd3BtfvDCpA' \
--compressed";

            var result = new CurlConverter().Convert(input);
            Assert.Equal(11, result.Headers.Count);
            Assert.Equal("authority", result.Headers[0].Key);
            Assert.Equal("appcenter.ms", result.Headers[0].Value);
        }

        [Fact]
        public void UrlTest()
        {
            var input = @"curl 'https://appcenter.ms/api/v0.1/apps/jenius_apps/ambie/analytics/languages/?start=2020-10-18T06%3A08%3A04.735Z&$top=7' \
-H 'authority: appcenter.ms' \
-H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36 Edg/86.0.622.69' \
-H 'diagnostic-context: 42ff43c8-a579-4e1d-8465-f8e644384f73' \
-H 'internal-request-source: portal' \
-H 'authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJLFaamoFOWPjGf54Jdy26JCtztq8RRGOgKIP6IwLiYjPb6Dg' \
-H 'accept: */*' \
-H 'sec-fetch-site: same-origin' \
-H 'sec-fetch-mode: cors' \
-H 'sec-fetch-dest: empty' \
-H 'accept-language: en-US,en;q=0.9' \
-H 'cookie: MSCC=1596513642; ai_user=J/3y0|2020-08-04T04:00:42.856Z; ajs_group_id=null; ajs_anonymous_id=%22c48c3860-7a21-4b2b-a723-0f49ca43a24c%22; _csrf=0Avm4Guu1Avd5PENLy5u5Vvf; ai_session=gQj0+|1605595865826|1605596884272.81; session=eyJ0aW1lc3RhbXAiOiIyMDIwLTExLTE3VDA3OjA4OjA0LjM0NFoiLCJmbGFzaCI6e30sImxvZ2luLXNvdXJjZSI6eyJ0eXBlIjoiYWFkIiwiaXNNaWNyb3NvZnRUZW5hbnQiOmZhbHNlfSwidXNlci1vcmlnaW4iOiJhcHBjZW50ZXIiLCJrbXNpIjp0cnVlLCJ0aWQiOiJmOGNkZWYzMS1hMzFlLTRiNGEtOTNlNC01ZjU3MWU5MTI1NWEiLCJwYXNzcG9ydCI6eyJ1c2VyIjoie1wiaWRcIjpcIjdmZjUwYWU5LWEyOWEtNDliOC05MTVjLTZhMzQyNGNmZTMwM1wifSJ9LCJzZXNzaW9uLWhhc2giOm51bGx9; session.sig=Sa6U_EdayPjfgumdUd3BtfvDCpA' \
--compressed";

            var result = new CurlConverter().Convert(input);
            Assert.Equal("https://appcenter.ms/api/v0.1/apps/jenius_apps/ambie/analytics/languages/?start=2020-10-18T06%3A08%3A04.735Z&$top=7", result.Url.Base);
        }

        [Fact]
        public void DoubleQuoteTest()
        {
            var input = @"curl -X PUT ""https://api.cloudflare.com/client/v4/user/tokens/ed17574386854bf78a67040be0a770b0"" \
-H ""Authorization: Bearer 8M7wS6hCpXVc-DoRnPPY_UCWPgy8aea4Wy6kCe5T"" \
-H ""Content-Type: application/json"" \
--data '{""id"":""ed17574386854bf78a67040be0a770b0"",""name"":""readonly token"",""status"":""active"",""issued_on"":""2018-07-01T05:20:00Z"",""modified_on"":""2018-07-02T05:20:00Z"",""not_before"":""2018-07-01T05:20:00Z"",""expires_on"":""2020-01-01T00:00:00Z"",""policies"":[{""id"":""f267e341f3dd4697bd3b9f71dd96247f"",""effect"":""allow"",""resources"":{""com.cloudflare.api.account.zone.eb78d65290b24279ba6f44721b3ea3c4"":""*"",""com.cloudflare.api.account.zone.22b1de5f1c0e4b3ea97bb1e963b06a43"":""*""},""permission_groups"":[{""id"":""c8fed203ed3043cba015a93ad1616f1f"",""name"":""Zone Read""},{""id"":""82e64a83756745bbbb1c9c2701bf816b"",""name"":""DNS Read""}]}],""condition"":{""request.ip"":{""in"":[""199.27.128.0/21"",""2400:cb00::/32""],""not_in"":[""199.27.128.0/21"",""2400:cb00::/32""]}}}'";

            var result = new CurlConverter().Convert(input);
            Assert.Equal("https://api.cloudflare.com/client/v4/user/tokens/ed17574386854bf78a67040be0a770b0", result.Url.Base);
        }

        [Fact]
        public void DataJsonTest()
        {
            var input = @"curl -X PUT ""https://api.cloudflare.com/client/v4/user/tokens/ed17574386854bf78a67040be0a770b0"" \
-H ""Authorization: Bearer 8M7wS6hCpXVc-DoRnPPY_UCWPgy8aea4Wy6kCe5T"" \
-H ""Content-Type: application/json"" \
--data '{""id"":""ed17574386854bf78a67040be0a770b0"",""name"":""readonly token"",""status"":""active"",""issued_on"":""2018-07-01T05:20:00Z"",""modified_on"":""2018-07-02T05:20:00Z"",""not_before"":""2018-07-01T05:20:00Z"",""expires_on"":""2020-01-01T00:00:00Z"",""policies"":[{""id"":""f267e341f3dd4697bd3b9f71dd96247f"",""effect"":""allow"",""resources"":{""com.cloudflare.api.account.zone.eb78d65290b24279ba6f44721b3ea3c4"":""*"",""com.cloudflare.api.account.zone.22b1de5f1c0e4b3ea97bb1e963b06a43"":""*""},""permission_groups"":[{""id"":""c8fed203ed3043cba015a93ad1616f1f"",""name"":""Zone Read""},{""id"":""82e64a83756745bbbb1c9c2701bf816b"",""name"":""DNS Read""}]}],""condition"":{""request.ip"":{""in"":[""199.27.128.0/21"",""2400:cb00::/32""],""not_in"":[""199.27.128.0/21"",""2400:cb00::/32""]}}}'";

            var result = new CurlConverter().Convert(input);
            Assert.Equal(RequestBodyType.Json, result.Body.BodyType);
            Assert.NotNull(result.Body.JsonBody);
        }

        [Fact]
        public void DataXmlTest()
        {
            var input = @"curl -X PUT ""https://api.cloudflare.com/client/v4/user/tokens/ed17574386854bf78a67040be0a770b0"" \
-H ""Authorization: Bearer 8M7wS6hCpXVc-DoRnPPY_UCWPgy8aea4Wy6kCe5T"" \
-H ""Content-Type: application/xml"" \
--data '{""id"":""ed17574386854bf78a67040be0a770b0"",""name"":""readonly token"",""status"":""active"",""issued_on"":""2018-07-01T05:20:00Z"",""modified_on"":""2018-07-02T05:20:00Z"",""not_before"":""2018-07-01T05:20:00Z"",""expires_on"":""2020-01-01T00:00:00Z"",""policies"":[{""id"":""f267e341f3dd4697bd3b9f71dd96247f"",""effect"":""allow"",""resources"":{""com.cloudflare.api.account.zone.eb78d65290b24279ba6f44721b3ea3c4"":""*"",""com.cloudflare.api.account.zone.22b1de5f1c0e4b3ea97bb1e963b06a43"":""*""},""permission_groups"":[{""id"":""c8fed203ed3043cba015a93ad1616f1f"",""name"":""Zone Read""},{""id"":""82e64a83756745bbbb1c9c2701bf816b"",""name"":""DNS Read""}]}],""condition"":{""request.ip"":{""in"":[""199.27.128.0/21"",""2400:cb00::/32""],""not_in"":[""199.27.128.0/21"",""2400:cb00::/32""]}}}'";

            var result = new CurlConverter().Convert(input);
            Assert.Equal(RequestBodyType.Xml, result.Body.BodyType);
            Assert.NotNull(result.Body.XmlBody);
        }

        [Fact]
        public void MethodTest()
        {
            var input = @"curl -X PUT ""https://api.cloudflare.com/client/v4/user/tokens/ed17574386854bf78a67040be0a770b0"" \
-H ""Authorization: Bearer 8M7wS6hCpXVc-DoRnPPY_UCWPgy8aea4Wy6kCe5T"" \
-H ""Content-Type: application/xml"" \
--data '{""id"":""ed17574386854bf78a67040be0a770b0"",""name"":""readonly token"",""status"":""active"",""issued_on"":""2018-07-01T05:20:00Z"",""modified_on"":""2018-07-02T05:20:00Z"",""not_before"":""2018-07-01T05:20:00Z"",""expires_on"":""2020-01-01T00:00:00Z"",""policies"":[{""id"":""f267e341f3dd4697bd3b9f71dd96247f"",""effect"":""allow"",""resources"":{""com.cloudflare.api.account.zone.eb78d65290b24279ba6f44721b3ea3c4"":""*"",""com.cloudflare.api.account.zone.22b1de5f1c0e4b3ea97bb1e963b06a43"":""*""},""permission_groups"":[{""id"":""c8fed203ed3043cba015a93ad1616f1f"",""name"":""Zone Read""},{""id"":""82e64a83756745bbbb1c9c2701bf816b"",""name"":""DNS Read""}]}],""condition"":{""request.ip"":{""in"":[""199.27.128.0/21"",""2400:cb00::/32""],""not_in"":[""199.27.128.0/21"",""2400:cb00::/32""]}}}'";

            var result = new CurlConverter().Convert(input);
            Assert.Equal("PUT", result.Method);
        }

        [Fact]
        public void DefaultMethodTest()
        {
            var input = @"curl https://api.discogs.com/releases/249504 --user-agent ""FooBarApp / 3.0""";

            var result = new CurlConverter().Convert(input);
            Assert.Equal("GET", result.Method);
        }

        [Fact]
        public void UserAgentTest()
        {
            var input = @"curl https://api.discogs.com/releases/249504 --user-agent ""FooBarApp / 3.0""";
            var result = new CurlConverter().Convert(input);
            Assert.Contains(result.Headers, x => x.Key == "User-Agent" && x.Value == "FooBarApp / 3.0");
        }
    }
}
