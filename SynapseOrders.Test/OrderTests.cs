using SynapseOrders.Models;

namespace SynapseOrders.Test
{
    // TODO I would add more tests here to check for excpetions
    public class OrderTests
    {
        [Fact]
        public void Order_ShouldThrowArgumentNullException_WhenOrderIdIsNull()
        {
            // Arrange
            var items = new List<Item> { new Item { Status = "Delivered", Description = "Test Item" } };
            var order = new Order { Items = items };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                order.OrderId = null;
                ValidateOrder(order);
            });
        }

        [Fact]
        public void Order_ShouldInitializeCorrectly_WhenValidParametersAreProvided()
        {
            // Arrange
            var items = new List<Item>
            {
                new() { Status = "Delivered", Description = "Test Item 1" },
                new() { Status = "Shipped", Description = "Test Item 2" }
            };

            var order = new Order
            {
                OrderId = "123",
                Items = items
            };

            // Act & Assert
            Assert.Equal("123", order.OrderId);
            Assert.Equal(2, order.Items.Count());
        }

        private void ValidateOrder(Order order)
        {
            if (order.OrderId == null)
            {
                throw new ArgumentNullException(nameof(order.OrderId));
            }

            if (order.Items == null)
            {
                throw new ArgumentNullException(nameof(order.Items));
            }

            if (!order.Items.Any())
            {
                throw new ArgumentException("Order must contain at least one item.", nameof(order.Items));
            }
        }
    }
}
