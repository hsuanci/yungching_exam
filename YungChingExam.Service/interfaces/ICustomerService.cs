using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetCustomerList();
    }
}
