using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpecFlow.AutoFixture.Tests
{
    public static class HttpClientExtensions
    {
        public static async Task<(T Result, HttpResponseMessage Response)> GetJsonResult<T>(
            this HttpClient source, string url, HttpMethod? method = null, object? content = null)
        {
            var request = new HttpRequestMessage(method ?? HttpMethod.Get, url)
            {
                Content = content == null ? null : new JsonContent(content)
            };
            var response = await source.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var dto = await response.ContentAs<T>();
            return (dto, response);
        }

        public static async Task<HttpResponseMessage> SendJsonContent(
            this HttpClient source, string url,
            HttpMethod method, object content,
            bool ensureSuccess = true)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = content == null ? null : new JsonContent(content)
            };
            var response = await source.SendAsync(request);

            if (ensureSuccess)
            {
                response.EnsureSuccessStatusCode();
            }

            return response;
        }

        public static async Task<T> ContentAs<T>(this HttpResponseMessage response)
        {
            var respContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings();

            return JsonConvert.DeserializeObject<T>(respContent, settings);
        }

        public static async Task<Guid> ContentAsGuid(this HttpResponseMessage response)
        {
            var respContent = await response.Content.ReadAsStringAsync();
            return Guid.Parse(respContent);
        }
    }
}
