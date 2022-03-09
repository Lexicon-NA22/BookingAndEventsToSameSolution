using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GymBooking.Web.Clients
{
    public class BaseClient
    {
        protected readonly HttpClient httpClient;
        protected readonly string mediaType;

        public BaseClient(HttpClient httpClient, Uri baseadress, string mediaType)
        {
            httpClient.BaseAddress = baseadress;
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            this.httpClient = httpClient;
            this.mediaType = mediaType;
        }

        public async Task<T> GetWithStreamsAsync<T>(CancellationToken token, string path, string contentType = "application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                response.EnsureSuccessStatusCode();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new Newtonsoft.Json.JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader)!;
                    }
                }
            }

        }
    }
}
