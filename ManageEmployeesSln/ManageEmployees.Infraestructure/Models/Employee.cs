using System.ComponentModel.DataAnnotations;

namespace ManageEmployees.Infraestructure.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public DateTime? ContractEndDate { get; set; }


    }
}
