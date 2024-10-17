namespace ManageEmployees.Core.DTOs
{
    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime HireDate { get; set; }
        public DateTime? ContractEndDate { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
    }
}
