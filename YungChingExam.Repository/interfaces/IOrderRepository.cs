using YungChingExam.Data.DTOs;
using YungChingExam.Data.Models;

namespace YungChingExam.Repository.interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrderQuery(int orderId);
        Task AddAsync(OrderDto orderDto);
        Task UpdateAsync(int orderId, OrderDto orderDto);
        Task DeleteAsync(int orderId);
    }
}
