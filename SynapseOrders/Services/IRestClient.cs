using Newtonsoft.Json.Linq;

namespace SynapseOrders.Services
{
    public interface IRestClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, string content);
    }
}