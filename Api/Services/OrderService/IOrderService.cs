using Api.Models;

namespace Api.Services.OrderService
{
    public interface IOrderService
    {
        (bool success, string message) CancelOrder(string orderCode);
        Task<(List<Order> orders, string message)> GetOrders();
    }
}
