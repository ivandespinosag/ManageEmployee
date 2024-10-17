using ManageEmployees.Infraestructure.Interfaces;
using ManageEmployees.Infraestructure.Models;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace ManageEmployees.Infraestructure.Data
{
    public class EmployeeData : IEmployeeData
    {
        private readonly ConnectionStrings _connection;

        public EmployeeData(IOptions<ConnectionStrings> options)
        {
            _connection = options.Value;
        }
        public async Task<List<Employee>> EmployeesList()
        {
            List<Employee> employees = new List<Employee>();

            using (var conection = new SqlConnection(_connection.SqlServerString))
            {
                await conection.OpenAsync();
                SqlCommand cmd = new SqlCommand("p_Get_Employees", conection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            ContractEndDate = Convert.ToDateTime(reader["contractEndDate"]),
                            Email = reader["email"].ToString()!,
                            EmployeeId = Convert.ToInt32(reader["employeeId"]),
                            FirstName = reader["firstName"].ToString()!,
                            HireDate = Convert.ToDateTime(reader["hireDate"]),
                            LastName = reader["lastName"].ToString()!,
                            PhotoUrl = reader["photoUrl"].ToString()!,
                            Position = reader["position"].ToString()!,
                        });
                    }
                }
            }

            return employees;
        }

        public async Task<int> AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            int employeeId;

            using (var connection = new SqlConnection(_connection.SqlServerString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("p_Add_Employee", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);
                    cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    cmd.Parameters.AddWithValue("@ContractEndDate", employee.ContractEndDate);
                    cmd.Parameters.AddWithValue("@PhotoUrl", employee.PhotoUrl);

                    // Add the output parameter
                    SqlParameter outputIdParam = new SqlParameter("@EmployeeId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    // Execute the command
                    await cmd.ExecuteNonQueryAsync();

                    // Get the generated EmployeeId
                    employeeId = (int)outputIdParam.Value;
                }
            }

            return employeeId;  // Return EmployeeId generate
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_connection.SqlServerString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("p_Delete_Employee", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UpdateEmployee(int employeeId, Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            using (var connection = new SqlConnection(_connection.SqlServerString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("p_Update_Employee", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);
                    cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
                    cmd.Parameters.AddWithValue("@ContractEndDate", employee.ContractEndDate);
                    cmd.Parameters.AddWithValue("@PhotoUrl", employee.PhotoUrl);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    return rowsAffected > 0; // Return true if will update successfull
                }
            }
        }
    }
}
