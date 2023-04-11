using Api.Models;
using Api.Services.DBService;
using static Api.Helper.Enums;

namespace Api.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IDbService _dbService;

        public OrderService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public (bool success, string message) CancelOrder(string orderCode)
        {
            var order =  _dbService.FirstOrDefault<Order>(x => x.OrderCode == orderCode);
            if (order == null)
            {
                return (false, $"Order with order code {orderCode} does not exist");
            }
            else if (order.Status == OrderStatus.Cancelled)
            {
                return (false, $"Order with order code {orderCode} is already cancelled");
            }
            else if (order.Status == OrderStatus.Delivered)
            {
                return (false, $"Order with order code {orderCode} is already delivered");
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
                try
                {
                 _dbService.Update(order);
                }
                catch (Exception ex)
                {
                    return (false, $"Failed to cancel order with order code {orderCode}");
                }
                
                return (true, $"Order with order code {orderCode} has been cancelled");
                
            }
        }

        public async Task<(List<Order> orders, string message)> GetOrders()
        {
            var orders = _dbService.Query<Order>(o => o.Status != OrderStatus.Cancelled ).ToList();
            return (orders, "");
        }
    }

}
