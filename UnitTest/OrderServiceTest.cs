using Api.Models;
using Api.Services.DBService;
using Api.Services.OrderService;
using Moq;
using NUnit.Framework;
using static Api.Helper.Enums;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using Autofac.Extras.Moq;

namespace UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Cancel Order test
        [Test]
        public async Task CancelOrder_OrderNotFound_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";

            using (var mock = AutoMock.GetLoose())
            {
                // set up the expected value of the FirstOrDefault method in IDbService
                mock.Mock<IDbService>().Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                    .Returns((Order)null);
                
                // create a mock instance of IOrderService
                var orderService = mock.Create<OrderService>();

                // Act
                (var success , var message) = orderService.CancelOrder(orderCode);

                // Assert
                Assert.IsFalse(success);
                Assert.AreEqual($"Order with order code {orderCode} does not exist", message);
            }
        }

        [Test]
        public async Task CancelOrder_OrderAlreadyCancelled_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";

            using (var mock = AutoMock.GetLoose())
            {
                var order = new Order { OrderCode = orderCode, Status = OrderStatus.Cancelled };
                mock.Mock<IDbService>().Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                    .Returns(order);

                var orderService = mock.Create<OrderService>();

                // Act
                (var success, var message) = orderService.CancelOrder(orderCode);

                // Assert
                Assert.IsFalse(success);
                Assert.AreEqual($"Order with order code {orderCode} is already cancelled", message);
            }
        }

        [Test]
        public async Task CancelOrder_OrderAlreadyDelivered_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";

            using (var mock = AutoMock.GetLoose())
            {
                var order = new Order { OrderCode = orderCode, Status = OrderStatus.Delivered };
                mock.Mock<IDbService>().Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                    .Returns(order);

                var orderService = mock.Create<OrderService>();

                // Act
                (var success, var message) = orderService.CancelOrder(orderCode);

                // Assert
                Assert.IsFalse(success);
                Assert.AreEqual($"Order with order code {orderCode} is already delivered", message);
            }
        }

        [Test]
        public async Task CancelOrder_SuccessfulCancellation_ReturnsSuccessMessage()
        {
            // Arrange
            string orderCode = "123";

            using (var mock = AutoMock.GetLoose())
            {
                var order = new Order { OrderCode = orderCode, Status = OrderStatus.New };
                mock.Mock<IDbService>().Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                    .Returns(order);

                var orderService = mock.Create<OrderService>();

                // Act
                (var success, var message) = orderService.CancelOrder(orderCode);

                // Assert
                mock.Mock<IDbService>().Verify(x => x.Update(It.IsAny<Order>()),Times.Once);
                Assert.IsTrue(success);
                Assert.AreEqual($"Order with order code {orderCode} has been cancelled", message);
            }
        }

        [Test]
        public async Task CancelOrder_FailedToCancelOrder_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";

            using (var mock = AutoMock.GetLoose())
            {
                var order = new Order { OrderCode = orderCode, Status = OrderStatus.InProgress };
                mock.Mock<IDbService>().Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                    .Returns(order);
                mock.Mock<IDbService>().Setup(x => x.Update(It.IsAny<Order>())).Throws(new Exception("Failed to cancel order"));

                var orderService = mock.Create<OrderService>();

                // Act
                (var success, var message) = orderService.CancelOrder(orderCode);

                // Assert
                Assert.IsFalse(success);
                Assert.AreEqual($"Failed to cancel order with order code {orderCode}", message);
            }
        }
    }
    #endregion
}