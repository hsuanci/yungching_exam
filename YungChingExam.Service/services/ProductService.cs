
using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetProductListAsync(bool? discontinued = false)
        {
            return await _productRepository.GetProductsQuery()
                .Where(x => x.Discontinued == discontinued)
                .AsNoTracking()
                .Select(x => new ProductDto()
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    QuantityPerUnit = x.QuantityPerUnit,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    UnitsOnOrder = x.UnitsOnOrder
                })
                .ToListAsync();
        }
    }
}
