using ManageEmployees.Infraestructure.Models;

namespace ManageEmployees.Infraestructure.Interfaces
{
    public interface IEmployeeData
    {
        Task<List<Employee>> EmployeesList();
        Task<int> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(int employeeId);
        Task<bool> UpdateEmployee(int employeeId, Employee employee);
    }
}
