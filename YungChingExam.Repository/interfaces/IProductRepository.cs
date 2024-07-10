using YungChingExam.Data.Models;

namespace YungChingExam.Repository.interfaces
{
    public interface IProductRepository
    {
        public IQueryable<Product> GetProductsQuery();
    }
}
