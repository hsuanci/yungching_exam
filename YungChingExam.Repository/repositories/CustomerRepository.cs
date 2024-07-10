using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;

namespace YungChingExam.Repository.repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly YungChingContext _yungChingContext;

        public CustomerRepository(YungChingContext yungChingContext)
        {
            _yungChingContext = yungChingContext;
        }

        public IQueryable<Customer> GetCustomersQuery()
        {
            return _yungChingContext.Customers;
        }
    }
}
