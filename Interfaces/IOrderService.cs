using Bakery.Models.DbModels;
using Bakery.Models.DTO;

namespace Bakery.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(CreateOrderRequest request);
        Task<List<OrderResponse>> GetOrders(long? orderId = null, string? phone = null);
        Task<long> CreateCustomer(CustomerDto customer);
        Task<bool> EditOrderItems(ChangeOrderRequest request);
        Task<bool> DeleteOrder(long orderId);
        Task<bool> ChangeOrderStatus(ChangeOrderStatusRequest request);
    }
}
