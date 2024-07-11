using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderDto dto, bool useCustomerCurrentAddressState);
        Task UpdateOrderAsync(int orderId, OrderDto dto, bool useCustomerCurrentAddressState);
        Task DeleteOrderAsync(int orderId);
    }
}
