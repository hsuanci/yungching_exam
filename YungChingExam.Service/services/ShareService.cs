
using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class ShareService : IShareService
    {
        private readonly IShipperRepository _shipperRepository;
        public ShareService(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
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
    }
}
