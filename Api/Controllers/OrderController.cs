using Api.Models;
using Api.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService 
            )
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<ApiResponse<List<Order>>> GetOrders()
        {
            // Call the OrderService to get the list of orders
            (var orders , var message) = await _orderService.GetOrders();
            if ( orders == null || orders.Count==0)
            {
                return ApiResponse<List<Order>>.CreateFailure("Orders are not found");
            }

                return ApiResponse<List<Order>>.Create(orders);
        }


        [HttpPost(Name = "CancelOrder")]
        public async Task<ApiResponse<bool>> CancelOrder(string OrderCode)
        {
            // Call the OrderService to get the list of orders
            (var success, var message) = _orderService.CancelOrder(OrderCode);
            if (!success)
            {
                return ApiResponse<bool>.CreateFailure(message);
            }

            return ApiResponse<bool>.Create(success);
        }
    }
}