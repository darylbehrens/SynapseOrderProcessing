using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SynapseOrders.Models;
using SynapseOrders.Services;

namespace SynapseOrders
{
    // If I had more time I would have
    // 1. Implemeneted Cancellation Tokens on the Async Calls
    // 2. Added Specific error handling for specific errors (HTTP Errors, Deserialization Errors
    // 3. Abstracted out all the API calls to their own "Manager" that was injected (OrdersManager, ItemsManager, etc..). This would also allow better Testing of the functions.
    // 4. If we had any config data in an appsettings.json I would use that instead of hard coded URL's for the endpoints
    // 5. I would have finished Test Suite
    // 6. I would have added more inline comments

    public class SynapseOrderProcessing(ILogger<SynapseOrderProcessing> _logger, IRestClient _restClient)
    {
        public async Task<OrderProcessingResult> ProcessOrders()
        {
            _logger.LogInformation("Processing Orders Started");
            var result = new OrderProcessingResult();

            try
            {
                // Grab All Orders
                var medicalEquipmentOrders = await FetchMedicalEquipmentOrders();
                result.TotalOrdersProcessed = medicalEquipmentOrders.Count();

                // Process and Send Alerts
                foreach (var order in medicalEquipmentOrders)
                {
                    bool isAlertSent = await SendAlert(order);
                    bool isOrderUpdated = isAlertSent && await UpdateOrder(order);

                    if (isAlertSent && isOrderUpdated)
                    {
                        result.SuccessfulOrders++;
                        _logger.LogInformation($"Order {order.OrderId} processed successfully.");
                    }
                    else
                    {
                        result.FailedOrders++;
                        _logger.LogError($"Order {order.OrderId} was not processed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical Error In Processing Order");
                throw;
            }
            finally
            {
                _logger.LogInformation("Processing Orders Ended");
            }

            return result;
        }

        private async Task<IEnumerable<Order>> FetchMedicalEquipmentOrders()
        {
            string ordersApiUrl = "https://orders-api.com/orders";

            try
            {
                var response = await _restClient.GetAsync(ordersApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var ordersData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<Order>>(ordersData) ?? [];
                }
                else
                {
                    var errorMessage = $"Failed to fetch orders from API. Status Code: {response.StatusCode}";
                    _logger.LogError(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error in processing orders.");
                throw;
            }
        }

        private async Task<bool> SendAlert(Order order)
        {
            if (order == null)
            {
                _logger.LogWarning("Order is null or has no items.");
                return false;
            }

            try
            {
                var itemsToAlert = order.Items?.Where(i => i.IsDelivered()).ToList() ?? [];

                if (itemsToAlert.Count == 0)
                {
                    _logger.LogInformation($"No delivered items to alert for order with ID {order.OrderId}");
                    return true;
                }

                foreach (var item in itemsToAlert)
                {
                    if (order.OrderId != null && await SendAlertMessage(item, order.OrderId))
                    {
                        item.IncrementDeliveryNotification();
                    }
                    else
                    {
                        _logger.LogError($"Failed to send alert for item: {item.Description}, Order ID: {order.OrderId}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing items for order with ID {order.OrderId}");
                throw;
            }
        }

        private async Task<bool> SendAlertMessage(Item item, string orderId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null");
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentNullException(nameof(orderId), "OrderId cannot be null or empty");

            try
            {
                string alertApiUrl = "https://alert-api.com/alerts";
                var alertData = new
                {
                    Message = $"Alert for delivered item: Order {orderId}, Item: {item.Description}, " +
                              $"Delivery Notifications: {item.DeliveryNotification}"
                };

                var jsonAlertData = JsonConvert.SerializeObject(alertData);
                var response = await _restClient.PostAsync(alertApiUrl, jsonAlertData);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Alert sent for delivered item: {item.Description}, Order ID: {orderId}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to send alert for delivered item: {item.Description}, Order ID: {orderId}. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Critical error while sending alert for item {item.Description} in order {orderId}");
                throw;
            }
        }


        private async Task<bool> UpdateOrder(Order order)
        {
            if (order == null)
            {
                _logger.LogWarning("Order is null. Cannot update.");
                return false;
            }

            try
            {
                string updateApiUrl = "https://update-api.com/update";
                var jsonOrderData = JsonConvert.SerializeObject(order);
                var response = await _restClient.PostAsync(updateApiUrl, jsonOrderData);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Order updated successfully: OrderId {order.OrderId}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to update order: OrderId {order.OrderId}. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Critical error while updating order with ID {order.OrderId}");
                throw;
            }
        }
    }
}