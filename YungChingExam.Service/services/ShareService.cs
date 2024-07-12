
using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class ShareService : IShareService
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IProductService _productService;

        public ShareService(IShipperRepository shipperRepository, IProductService productService)
        {
            _shipperRepository = shipperRepository;
            _productService = productService;
        }

        public async Task<List<ShipperDto>> GetShipperListAsync()
        {
            return await _shipperRepository.GetShippersQuery()
                .AsNoTracking()
                .Select(x => new ShipperDto()
                {
                    ShipperId = x.ShipperId,
                    CompanyName = x.CompanyName,
                    Phone = x.Phone

                })
                .ToListAsync();
        }

        public DILifeCycleDto GetInstanceHashCode()
        {
            return new DILifeCycleDto
            {
                InstaceOneHash = _productService.GetShipperRepositoryHash(),
                InstaceTwoHash = _shipperRepository.GetHashCode(),
            };
        }
    }
}
