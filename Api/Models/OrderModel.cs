using static Api.Helper.Enums;

namespace Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
