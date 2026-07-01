using CRUDrequestsTask1.Data;
using CRUDrequestsTask1.Models;
using Microsoft.Extensions.Configuration;

namespace CRUDrequestsTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var adoRepo = new AdoEmployeeRepository(connectionString);
            var efContext = new ApplicationDbContext();
            var efRepo = new EfEmployeeRepository(efContext);

            while (true)
            {
                Console.WriteLine("Выберите режим:");
                Console.WriteLine("1. ADO.NET");
                Console.WriteLine("2. Entity Framework");
                Console.WriteLine("3. Exit");
                Console.Write("--> ");
                string mode = Console.ReadLine();

                if (mode == "3")
                    break;

                Console.WriteLine("\nОперация:");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read all");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Back");
                Console.Write("--> ");
                string action = Console.ReadLine();

                if (mode == "1")
                {
                    DoAdo(action, adoRepo);
                }
                else if (mode == "2")
                {
                    DoEf(action, efRepo);
                }

                Console.WriteLine();
            }

            static Employee ReadEmployee()
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

            static void DoAdo(string action, AdoEmployeeRepository repo)
            {
                switch (action)
                {
                    case "1":
                        repo.Create(ReadEmployee());
                        Console.WriteLine("Создано при помощи ADO.NET");
                        break;

                    case "2":
                        foreach (var e in repo.GetAll())
                            Console.WriteLine($"{e.Id}: {e.FirstName} {e.LastName} {e.Email} {e.Salary}");
                        break;

                    case "3":
                        var upd = ReadEmployee();
                        Console.Write("Id: ");
                        upd.Id = int.Parse(Console.ReadLine());
                        repo.Update(upd);
                        Console.WriteLine("Обновлено при помощи ADO.NET");
                        break;

                    case "4":
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());
                        repo.Delete(id);
                        Console.WriteLine("Удалено при помощи ADO.NET");
                        break;

                    case "5":
                        break;
                }
            }

            static void DoEf(string action, EfEmployeeRepository repo)
            {
                switch (action)
                {
                    case "1":
                        repo.Create(ReadEmployee());
                        Console.WriteLine("Создано при помощи EF");
                        break;

                    case "2":
                        foreach (var e in repo.GetAll())
                            Console.WriteLine($"{e.Id}: {e.FirstName} {e.LastName} {e.Email} {e.Salary}");
                        break;

                    case "3":
                        var upd = ReadEmployee();
                        Console.Write("Id: ");
                        upd.Id = int.Parse(Console.ReadLine());
                        repo.Update(upd);
                        Console.WriteLine("Обновлено при помощи EF");
                        break;

                    case "4":
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());
                        repo.Delete(id);
                        Console.WriteLine("Удалено при помощи EF");
                        break;
                }
            }

        }
    }
}