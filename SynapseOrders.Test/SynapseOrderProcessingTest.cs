using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SynapseOrders.Models;
using SynapseOrders.Services;

namespace SynapseOrders.Test
{
    // If I had more time I would include More tests for exceptions
    public class SynapseOrderProcessingTests
    {
        private readonly Mock<ILogger<SynapseOrderProcessing>> _loggerMock;
        private readonly Mock<IRestClient> _restClientMock;
        private readonly SynapseOrderProcessing _orderProcessing;

        public SynapseOrderProcessingTests()
        {
            _loggerMock = new Mock<ILogger<SynapseOrderProcessing>>();
            _restClientMock = new Mock<IRestClient>();
            _orderProcessing = new SynapseOrderProcessing(_loggerMock.Object, _restClientMock.Object);
        }

        [Fact]
        public async Task ProcessOrders_ShouldReturnCorrectResult_WhenSomeOrdersFail()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = "123",
                    Items = new List<Item>
                    {
                        new Item { Status = "Delivered", Description = "Item1", DeliveryNotification = 0 }
                    }
                },
                new Order
                {
                    OrderId = "456",
                    Items = new List<Item>
                    {
                        new Item { Status = "Delivered", Description = "Item2", DeliveryNotification = 0 }
                    }
                }
            };

            _restClientMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(orders))
                });

            _restClientMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string content) =>
                {
                    if (content.Contains("Item1"))
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.OK); // Success for Item1
                    }
                    else if (content.Contains("Item2"))
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest); // Fail for Item2
                    }
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                });


            // Act
            var result = await _orderProcessing.ProcessOrders();

            // Assert
            Assert.Equal(2, result.TotalOrdersProcessed);
            Assert.Equal(1, result.SuccessfulOrders);
            Assert.Equal(1, result.FailedOrders);
            Assert.False(result.IsSuccessful);

            // Verify PostAsync was called three times
            _restClientMock.Verify(x => x.PostAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
