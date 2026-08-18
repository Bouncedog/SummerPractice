using CRUDrequestsTask1.Models;

namespace CRUDrequestsTask1.Services
{
    internal class ServicesConsole
    {
        public static int ReadId()
        {
            Console.Write("Id: ");

            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                throw new ArgumentException("Id должен быть положительным целым числом.");

            return id;
        }

        public static Employee CreateEmployee()
        {
            Console.Write("FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            return new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Salary = salary
            };
        }

        public static Employee UpdateEmployee(int id)
        {
            Console.Write("FirstName: ");
            string firstName = Console.ReadLine();

            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            return new Employee
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Salary = salary
            };
        }
    }
}
