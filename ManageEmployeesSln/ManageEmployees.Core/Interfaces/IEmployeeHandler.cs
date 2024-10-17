using ManageEmployees.Core.DTOs;

namespace ManageEmployees.Core.Interfaces
{
    public interface IEmployeeHandler
    {
        Task<List<EmployeeDto>> GetEmployees();
        Task<int> AddEmployee(EmployeeAddDto employeeDto);
        Task<bool> DeleteEmployee(int employeeId);
        Task<bool> UpdateEmployee(int employeeId, EmployeeUpdateDto employeeDto);
    }
}
