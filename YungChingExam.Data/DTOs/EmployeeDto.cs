namespace YungChingExam.Data.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleTitle { get; set; }
        public string PhotoPath { get; set; }
    }

    public class EmployeeAuthResult
    {
        public bool IsAuthenticated { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
