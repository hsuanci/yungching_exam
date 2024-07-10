using YungChingExam.Data.Models;
using YungChingExam.Repository.interfaces;

namespace YungChingExam.Repository.repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly YungChingContext _yungChingContext;

        public EmployeeRepository(YungChingContext yungChingContext)
        {
            _yungChingContext = yungChingContext;
        }

        public IQueryable<Employee> GetEmployeeQuery()
        {
            return _yungChingContext.Employees;
        }
    }
}
