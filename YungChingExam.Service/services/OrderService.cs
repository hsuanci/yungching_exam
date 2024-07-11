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
