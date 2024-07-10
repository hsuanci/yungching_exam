using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;

namespace YungChingExam.Repository.repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly YungChingContext _yungChingContext;

        public ProductRepository(YungChingContext yungChingContext)
        {
            _yungChingContext = yungChingContext;
        }

        public IQueryable<Product> GetProductsQuery()
        {
            return _yungChingContext.Products;
        }
    }
}
