using System.Text.Json.Serialization;

namespace SynapseOrders.Models
{
    public class Order
    {
        [JsonRequired]
        public IEnumerable<Item>? Items { get; set; }

        [JsonRequired]
        public string? OrderId { get; set; }
    }
}