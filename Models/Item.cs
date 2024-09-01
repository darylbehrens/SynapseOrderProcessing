using System.Text.Json.Serialization;

namespace SynapseOrders.Models
{
    public class Item
    {
        [JsonRequired]
        public string? Status { get;  set; }

        [JsonRequired]
        public string? Description { get; set; }

        [JsonRequired]
        [JsonPropertyName("deliveryNotification")]
        public int DeliveryNotification { get; set; }

        public bool IsDelivered() => Status != null && Status.Equals("Delivered", StringComparison.OrdinalIgnoreCase);
        public void IncrementDeliveryNotification() => DeliveryNotification++;
    }

}