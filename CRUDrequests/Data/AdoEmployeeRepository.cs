using CRUDrequestsTask1.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUDrequestsTask1.Data
{
    public class AdoEmployeeRepository
    {
        private readonly string _connectionString;

        public AdoEmployeeRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // CREATE
        public void Create(Employee employee)
        {
            const string sql = @"
                INSERT INTO Employees (FirstName, LastName, Email, Salary)
                VALUES (@FirstName, @LastName, @Email, @Salary);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = employee.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = employee.LastName;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = employee.Email;
            command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;

            connection.Open();
            command.ExecuteNonQuery();
        }

        // READ: все
        public List<Employee> GetAll()
        {
            const string sql = @"
                SELECT Id, FirstName, LastName, Email, Salary
                FROM Employees;";

            var result = new List<Employee>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var employee = new Employee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Salary = reader.GetDecimal(reader.GetOrdinal("Salary"))
                };

                result.Add(employee);
            }

            return result;
        }

        // UPDATE
        public void Update(Employee employee)
        {
            const string sql = @"
                UPDATE Employees
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    Salary = @Salary
                WHERE Id = @Id;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = employee.FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = employee.LastName;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = employee.Email;
            command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;
            command.Parameters.Add("@Id", SqlDbType.Int).Value = employee.Id;

            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE
        public void Delete(int id)
        {
            const string sql = @"DELETE FROM Employees WHERE Id = @Id;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}