using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly YungChingContext _yungChingContext;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            YungChingContext yungChingContext)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _yungChingContext = yungChingContext;
        }

        public async Task<OrderPaginationDto> GetOrderListAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _orderRepository
                             .GetOrderQuery()
                             .CountAsync();

            var orderList = await _orderRepository
                .GetOrderQuery()
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.ShipViaNavigation)
                .OrderByDescending(x => x.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(order => new OrderDto
                {
                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer.ContactName,
                    EmployeeId = order.EmployeeId,
                    EmployeeName = order.Employee.FirstName + " " + order.Employee.LastName,
                    OrderDate = order.OrderDate,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    ShipperCompanyName = order.ShipViaNavigation.CompanyName,
                    ShipVia = order.ShipVia,
                    Freight = order.Freight,
                    ShipName = order.ShipName,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipRegion = order.ShipRegion,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipCountry = order.ShipCountry,
                    OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDto
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product.ProductName,
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Discount = detail.Discount
                    }).ToList()
                })
                .ToListAsync();

            return new OrderPaginationDto
            {
                Orders = orderList,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber
            };
        }

        public async Task CreateOrderAsync(OrderDto dto, bool useCustomerCurrentAddressState)
        {
            using (var transaction = await _yungChingContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 限縮查詢 ProductIds > 效能
                    var orderProductIds = dto.OrderDetails
                        .Select(x => x.ProductId)
                        .ToList();

                    // 獲取所有未停產產品的 ProductId 和 UnitPrice
                    var products = await _productRepository
                        .GetProductsQuery()
                        .Where(x => x.UnitPrice != null && !x.Discontinued && orderProductIds.Contains(x.ProductId))
                        .ToListAsync();

                    // 重新 mapping price and discount
                    var discount = 0.1f; // 從數據庫中獲取折扣值
                    dto.OrderDetails = this.MappingProductDiscount(discount, dto.OrderDetails, products.ToDictionary(p => p.ProductId, p => (decimal)p.UnitPrice));

                    // 是否使用 customer address for shipper
                    if (useCustomerCurrentAddressState)
                    {
                        var customerInfo = await _customerRepository
                            .GetCustomersQuery()
                            .AsNoTracking()
                            .FirstAsync(x => x.CustomerId == dto.CustomerId);

                        dto.ShipName = customerInfo.ContactTitle;
                        dto.ShipAddress = customerInfo.Address;
                        dto.ShipCity = customerInfo.City;
                        dto.ShipRegion = customerInfo.Region;
                        dto.ShipPostalCode = customerInfo.PostalCode;
                        dto.ShipCountry = customerInfo.Country;
                    }

                    // 更新庫存
                    this.UpdateProductInStock(products, dto.OrderDetails);

                    await _orderRepository.AddAsync(dto);

                    await _yungChingContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred during the transaction. See inner exception for details.", ex);
                }
            }
        }

        public async Task UpdateOrderAsync(int orderId, OrderDto dto, bool useCustomerCurrentAddressState)
        {
            using (var transaction = await _yungChingContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 獲取所有未停產產品的 ProductId 和 UnitPrice
                    var products = await _productRepository
                        .GetProductsQuery()
                        .Where(x => x.UnitPrice != null && !x.Discontinued)
                        .ToListAsync();

                    // 重新 mapping price and discount
                    var discount = 0.1f; // 從數據庫中獲取折扣值
                    dto.OrderDetails = this.MappingProductDiscount(discount, dto.OrderDetails, products.ToDictionary(p => p.ProductId, p => (decimal)p.UnitPrice));

                    // 是否使用 customer address for shipper
                    if (useCustomerCurrentAddressState)
                    {
                        var customerInfo = await _customerRepository
                            .GetCustomersQuery()
                            .AsNoTracking()
                            .FirstAsync(x => x.CustomerId == dto.CustomerId);

                        dto.ShipName = customerInfo.ContactTitle;
                        dto.ShipAddress = customerInfo.Address;
                        dto.ShipCity = customerInfo.City;
                        dto.ShipRegion = customerInfo.Region;
                        dto.ShipPostalCode = customerInfo.PostalCode;
                        dto.ShipCountry = customerInfo.Country;
                    }

                    // 更新庫存
                    var order = await _orderRepository
                        .GetOrderQuery()
                        .FirstAsync(x => x.OrderId == orderId);

                    this.RecoverProductInStock(products, order.OrderDetails.ToList());

                    this.UpdateProductInStock(products, dto.OrderDetails);

                    await _orderRepository.UpdateAsync(orderId, dto);

                    await _yungChingContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred during the transaction. See inner exception for details.", ex);
                }
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            using (var transaction = await _yungChingContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _orderRepository
                      .GetOrderQuery()
                      .FirstAsync(x => x.OrderId == orderId);

                    var products = await _productRepository
                        .GetProductsQuery()
                        .Where(x => x.UnitPrice != null && !x.Discontinued)
                        .ToListAsync();

                    // 已出貨無法刪除
                    if (order.ShippedDate.HasValue)
                        throw new Exception("This order has already been shipped.");

                    // 還原庫存
                    this.RecoverProductInStock(products, order.OrderDetails.ToList());

                    await _orderRepository.DeleteAsync(orderId);

                    await _yungChingContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred during the transaction. See inner exception for details.", ex);
                }
            }
        }

        #region private
        private List<OrderDetailDto> MappingProductDiscount(float discount, List<OrderDetailDto> detailDtos, Dictionary<int, decimal> producDic)
        {
            return detailDtos.Select(x => new OrderDetailDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = producDic.ContainsKey(x.ProductId) ? producDic[x.ProductId] : 0,
                Discount = discount,
            }).ToList();
        }

        private void RecoverProductInStock(List<Product> products, List<OrderDetail> detailDtos)
        {
            foreach (var detailDto in detailDtos)
            {
                var product = products.FirstOrDefault(x => x.ProductId == detailDto.ProductId);

                if (product == null)
                    throw new Exception($"Product with ID {detailDto.ProductId} not found");

                // 恢復庫存數量
                product.UnitsInStock = (short)(product.UnitsInStock + detailDto.Quantity);
            }
        }

        private void UpdateProductInStock(List<Product> products, List<OrderDetailDto> detailDtos)
        {
            foreach (var detailDto in detailDtos)
            {
                var product = products.First(x => x.ProductId == detailDto.ProductId);

                // 檢查是否會導致庫存數量低於零
                if (product.UnitsInStock < detailDto.Quantity)
                    throw new Exception("Product not enough for this order");

                // 更新庫存數量
                product.UnitsInStock = (short)(product.UnitsInStock - detailDto.Quantity);
            }

        }
        #endregion
    }
}
