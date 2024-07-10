using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;

namespace YungChingExam.Repository.repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly YungChingContext _yungChingContext;

        public ShipperRepository(YungChingContext yungChingContext)
        {
            _yungChingContext = yungChingContext;
        }

        public IQueryable<Shipper> GetShippersQuery()
        {
            return _yungChingContext.Shippers;
        }
    }
}
