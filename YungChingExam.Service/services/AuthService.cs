using Microsoft.EntityFrameworkCore;
using YungChingExam.Data.DTOs;
using YungChingExam.Repository.interfaces;
using YungChingExam.Service.interfaces;

namespace YungChingExam.Service.services
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeAuthResult> AuthenticateEmployeeAsync(string userAccount, string password)
        {
            var employee = await _employeeRepository
                          .GetEmployeeQuery()
                          .AsNoTracking()
                          .FirstOrDefaultAsync(x => x.FirstName == userAccount && x.Extension == password);

            if (employee == null)
                return new EmployeeAuthResult() { IsAuthenticated = false };

            return new EmployeeAuthResult()
            {
                IsAuthenticated = true,
                Employee = new EmployeeDto()
                {
                    Id = employee.EmployeeId,
                    Name = $"{employee.FirstName}{employee.LastName}",
                    RoleTitle = employee.Title,
                    PhotoPath = employee.PhotoPath
                }
            };
        }
    }
}
