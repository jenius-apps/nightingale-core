using JeniusApps.Nightingale.Converters.Curl;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
