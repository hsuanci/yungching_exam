using YungChingExam.Data.Models;

namespace YungChingExam.Repository.interfaces
{
    public interface IShipperRepository
    {
        public IQueryable<Shipper> GetShippersQuery();
    }
}
