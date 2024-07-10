using YungChingExam.Data.Models;

namespace YungChingExam.Repository.interfaces
{
    public interface IEmployeeRepository
    {
        public IQueryable<Employee> GetEmployeeQuery();
    }
}
