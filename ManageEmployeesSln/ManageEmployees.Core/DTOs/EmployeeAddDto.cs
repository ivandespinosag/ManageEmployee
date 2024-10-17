namespace ManageEmployees.Core.DTOs
{
    public class EmployeeAddDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
