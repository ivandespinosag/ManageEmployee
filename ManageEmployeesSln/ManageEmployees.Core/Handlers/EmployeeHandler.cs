using AutoMapper;
using ManageEmployees.Core.DTOs;
using ManageEmployees.Core.Interfaces;
using ManageEmployees.Infraestructure.Interfaces;
using ManageEmployees.Infraestructure.Models;

namespace ManageEmployees.Core.Handlers
{
    public class EmployeeHandler : IEmployeeHandler
    {
        private readonly IEmployeeData _employeeData;
        private readonly IMapper _mapper;

        public EmployeeHandler(IEmployeeData employeeData, IMapper mapper)
        {
            _employeeData = employeeData;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            var employees = await _employeeData.EmployeesList();
            var employeesDto = _mapper.Map<List<EmployeeDto>>(employees);
            return employeesDto;
        }

        public async Task<int> AddEmployee(EmployeeAddDto employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto));

            var employee = _mapper.Map<Employee>(employeeDto);
            int employeeId = await _employeeData.AddEmployee(employee);
            return employeeId;
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            return await _employeeData.DeleteEmployee(employeeId);
        }

        public async Task<bool> UpdateEmployee(int employeeId, EmployeeUpdateDto employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto));

            var employee = _mapper.Map<Employee>(employeeDto);
            return await _employeeData.UpdateEmployee(employeeId, employee);
        }
    }
}
