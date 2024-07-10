
using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDto>> GetCustomerList()
        {
            return await _customerRepository.GetCustomersQuery()
                           .AsNoTracking()
                           .Select(x => new CustomerDto
                           {
                               Fax = x.Fax,
                               ContactName = x.ContactName,
                               CustomerId = x.CustomerId,
                               CompanyName = x.CompanyName,
                               ContactTitle = x.ContactTitle,
                               Address = x.Address,
                               City = x.City,
                               Region = x.Region,
                               PostalCode = x.PostalCode,
                               Country = x.Country,
                               Phone = x.Phone
                           })
                           .ToListAsync();
        }
    }
}
