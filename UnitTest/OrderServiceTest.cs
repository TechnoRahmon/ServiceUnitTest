using Api.Models;
using Api.Services.DBService;
using Api.Services.OrderService;
using Moq;
using NUnit.Framework;
using static Api.Helper.Enums;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace UnitTest
{
    public class Tests
    {
        private Mock<IDbService> _dbServiceMock;
        private IOrderService orderService;
        [SetUp]
        public void Setup()
        {
            _dbServiceMock = new Mock<IDbService>();
            orderService = new OrderService(_dbServiceMock.Object);
        }

        #region Cancel Order test
        [Test]
        public async Task CancelOrder_OrderNotFound_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";
            _dbServiceMock.Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns((Order)null);

            // Act
            (var success , var message) = orderService.CancelOrder(orderCode);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual($"Order with order code {orderCode} does not exist", message);
        }

        [Test]
        public async Task CancelOrder_OrderAlreadyCancelled_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";
            var order = new Order { OrderCode = orderCode, Status = OrderStatus.Cancelled };
            _dbServiceMock.Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(order);

            // Act
            (var success, var message) =  orderService.CancelOrder(orderCode);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual($"Order with order code {orderCode} is already cancelled", message);
        }

        [Test]
        public async Task CancelOrder_OrderAlreadyDelivered_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";
            var order = new Order { OrderCode = orderCode, Status = OrderStatus.Delivered };
            _dbServiceMock.Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(order);

            // Act
            (var success, var message) = orderService.CancelOrder(orderCode);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual($"Order with order code {orderCode} is already delivered", message);
        }

        [Test]
        public async Task CancelOrder_SuccessfulCancellation_ReturnsSuccessMessage()
        {
            // Arrange
            string orderCode = "123";
            var order = new Order { OrderCode = orderCode, Status = OrderStatus.New };
            _dbServiceMock.Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(order);
            _dbServiceMock.Setup(x => x.Update(It.IsAny<Order>()));

            // Act
            (var success, var message) = orderService.CancelOrder(orderCode);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual($"Order with order code {orderCode} has been cancelled", message);
        }

        [Test]
        public async Task CancelOrder_FailedToCancelOrder_ReturnsErrorMessage()
        {
            // Arrange
            string orderCode = "123";
            var order = new Order { OrderCode = orderCode, Status = OrderStatus.InProgress };
            _dbServiceMock.Setup(x => x.FirstOrDefault<Order>(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns(order);
            _dbServiceMock.Setup(x => x.Update(It.IsAny<Order>())).Throws(new Exception("Failed to cancel order"));

            // Act
            (var success, var message) = orderService.CancelOrder(orderCode);

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual($"Failed to cancel order with order code {orderCode}", message);
        }
    }
    #endregion
}