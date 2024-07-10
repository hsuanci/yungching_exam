using YungChingExam.Data.Models;

namespace YungChingExam.Repository.interfaces
{
    public interface ICustomerRepository
    {
        public IQueryable<Customer> GetCustomersQuery();
    }
}
