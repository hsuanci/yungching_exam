using YungChingExam.Data.DTOs;

namespace YungChingExam.Service.interfaces
{
    public interface IAuthService
    {
        Task<EmployeeAuthResult> AuthenticateEmployeeAsync(string userAccount, string password);
    }
}
