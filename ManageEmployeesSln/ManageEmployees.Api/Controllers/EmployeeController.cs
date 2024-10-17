using ManageEmployees.Core.DTOs;
using ManageEmployees.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManageEmployees.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeHandler _employeeHandler;

        public EmployeeController(IEmployeeHandler employeeHandler)
        {
            _employeeHandler = employeeHandler;
        }

        [HttpGet("GetEmployees", Name = "GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employeesDto = await _employeeHandler.GetEmployees();
            return StatusCode(StatusCodes.Status200OK, employeesDto);
        }

        [HttpPost("PostEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeAddDto employee)
        {
            if (employee == null)
            {
                return BadRequest("Empleado no puede ser nulo");
            }

            int employeeId = await _employeeHandler.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployees), new { id = employeeId }, employee);
        }

        [HttpDelete("DeleteEmployee/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest("EmployeeId no es válido.");
            }

            var result = await _employeeHandler.DeleteEmployee(employeeId);

            if (result)
            {
                return NoContent(); // Retorna 204
            }
            else
            {
                return NotFound("El empleado no fue encontrado.");
            }
        }

        [HttpPut("PutEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto employee)
        {
            if (employee == null)
            {
                return BadRequest("Empleado no puede ser nulo");
            }

            var result = await _employeeHandler.UpdateEmployee(id, employee);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound("El empleado no fue encontrado.");
            }
        }

    }
}
