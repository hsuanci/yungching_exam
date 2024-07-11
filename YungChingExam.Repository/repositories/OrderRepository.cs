using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;

namespace YungChingExam.Repository.repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly YungChingContext _yungChingContext;

        public OrderRepository(YungChingContext yungChingContext)
        {
            _yungChingContext = yungChingContext;
        }

        public IQueryable<Order> GetOrderQuery(int orderId)
        {
            return _yungChingContext.Orders
                    .Include(x => x.OrderDetails)
                        .ThenInclude(x => x.Product)
                   .AsNoTracking()
                   .Where(x => x.OrderId == orderId);
        }

        public async Task AddAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                EmployeeId = orderDto.EmployeeId,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                ShipVia = orderDto.ShipVia,
                Freight = orderDto.Freight,
                ShipName = orderDto.ShipName,
                ShipAddress = orderDto.ShipAddress,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipCountry = orderDto.ShipCountry,
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount
                }).ToList()
            };

            await _yungChingContext.Orders.AddAsync(order);
        }

        public async Task UpdateAsync(int orderId, OrderDto orderDto)
        {
            var order = await _yungChingContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.EmployeeId = orderDto.EmployeeId;
            order.RequiredDate = orderDto.RequiredDate;
            order.ShippedDate = orderDto.ShippedDate;
            order.ShipVia = orderDto.ShipVia;
            order.Freight = orderDto.Freight;
            order.ShipName = orderDto.ShipName;
            order.ShipAddress = orderDto.ShipAddress;
            order.ShipCity = orderDto.ShipCity;
            order.ShipRegion = orderDto.ShipRegion;
            order.ShipPostalCode = orderDto.ShipPostalCode;
            order.ShipCountry = orderDto.ShipCountry;

            order.OrderDetails.Clear();
            foreach (var orderDetailDto in orderDto.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = orderDetailDto.ProductId,
                    UnitPrice = orderDetailDto.UnitPrice,
                    Quantity = orderDetailDto.Quantity,
                    Discount = orderDetailDto.Discount
                });
            }
        }

        public async Task DeleteAsync(int orderId)
        {
            var order = await _yungChingContext.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            _yungChingContext.Orders.Remove(order);
        }
    }
}
