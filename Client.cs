using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CTA_Tracker_Client
{
    public class Client
    {

        private HttpClient httpClient { get; }

        public string APIKey { get; }

        public Client(string apiKey)
        {
            APIKey = apiKey;

            // Init client
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(@"http://lapi.transitchicago.com/api/1.0/ttarrivals.aspx");
        }

        private async Task<Response> processRequest(string reqStr)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(reqStr);

                if (response.IsSuccessStatusCode)
                {
                    // Read reply

                    string replyString = await response.Content.ReadAsStringAsync();

                    // Convert to Response POCO

                    var buffer = Encoding.UTF8.GetBytes(replyString);
                    using (var stream = new MemoryStream(buffer))
                    {
                        var serializer = new XmlSerializer(typeof(Response));
                        var ctaResponse = (Response)serializer.Deserialize(stream);
                        return ctaResponse;
                    }
                }
                else
                    throw new HttpRequestException("unknown Request Error");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response> RequestUpdateByStationID(ushort stationId, int? maxResults = null, RouteColor? routeCode = null)
        {
            var reqStr = $"?key={APIKey}&mapid={stationId}";

            if (maxResults.HasValue)
                reqStr += $"&max={maxResults}";

            if (routeCode.HasValue)
                reqStr += $"&rt={routeCode.ToString().ToLower()}";

            var response = await processRequest(reqStr);

            return response;
        }

        public async Task<Response> RequestUpdateByStopID(ushort stopId, int? maxResults = null, RouteColor? routeCode = null)
        {
            var reqStr = $"?key={APIKey}&stpid={stopId}";

            if (maxResults.HasValue)
                reqStr += $"&max={maxResults}";

            if (routeCode.HasValue)
                reqStr += $"&rt={routeCode.ToString().ToLower()}";

            var response = await processRequest(reqStr);

            return response;
        }

    }
}
