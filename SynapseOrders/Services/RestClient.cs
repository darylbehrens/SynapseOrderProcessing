using Microsoft.Extensions.Logging;

namespace SynapseOrders.Services
{
    public class RestClient(ILogger<RestClient> _logger, IHttpClientFactory _httpClient) : IRestClient
    {
        public async Task<HttpResponseMessage> GetAsync(string url) =>await SendRequestAsync(HttpMethod.Get, url);

        public async Task<HttpResponseMessage> PostAsync(string url, string content) =>  await SendRequestAsync(HttpMethod.Post, url, content);

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, string? content = null)
        {
            try
            {
                var client = _httpClient.CreateClient();
                var request = new HttpRequestMessage(method, url);

                if (content != null)
                {
                    request.Content = new StringContent(content.ToString(), System.Text.Encoding.UTF8, "application/json");
                }

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"{method} request to {url} was successful.");
                    return response;
                }
                else
                {
                    _logger.LogWarning($"{method} request to {url} failed. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during {method} request to {url}.");
                throw;
            }
        }
    }
}