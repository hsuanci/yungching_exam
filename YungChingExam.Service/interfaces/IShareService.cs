using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface IShareService
    {
        Task<List<ShipperDto>> GetShipperList();
    }
}
