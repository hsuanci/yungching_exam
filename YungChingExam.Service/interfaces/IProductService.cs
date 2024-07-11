using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductListAsync(bool? discontinued = false);
    }
}
