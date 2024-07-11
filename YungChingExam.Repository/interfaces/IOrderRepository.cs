using YungChingExam.Data.DTOs;

namespace YungChingExam.Repository.interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(OrderDto orderDto);
        Task UpdateAsync(int orderId, OrderDto orderDto);
        Task DeleteAsync(int orderId);
    }
}
