using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderDto dto, bool useCustomerCurrentAddressState);
    }
}
