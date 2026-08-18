using CRUDrequestsTask1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CRUDrequestsTask1.Services;

namespace CRUDrequestsTask1;

internal class Program
{
    public static void InterfaceCRUD()
    {
        Console.WriteLine("\nОперация:");
        Console.WriteLine("1. Create");
        Console.WriteLine("2. Read all");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. Delete");
        Console.WriteLine("5. Back");
        Console.Write("--> ");
    }

    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Строка подключения DefaultConnection не найдена.");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

        using var efContext = new ApplicationDbContext(options);
        var adoRepo = new AdoEmployeeRepository(connectionString);
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

            if (mode == "1")
            {
                DoAdo(adoRepo);
                Console.WriteLine();
            }
            else if (mode == "2")
            {
                DoEf(efRepo);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Неверный режим.");
                Console.WriteLine();
            }
        }

        static void DoAdo(AdoEmployeeRepository repo)
        {
            InterfaceCRUD();

            string action = Console.ReadLine();
            Console.WriteLine();

            switch (action)
            {
                case "1":
                    repo.Create(ServicesConsole.CreateEmployee());

                    Console.WriteLine("Создано при помощи ADO.NET");
                    break;

                case "2":
                    foreach (var e in repo.GetAll())
                        Console.WriteLine($"{e.Id}: {e.FirstName} {e.LastName} {e.Email} {e.Salary:F2}");
                    break;

                case "3":
                    repo.Update(ServicesConsole.UpdateEmployee(ServicesConsole.ReadId()));

                    Console.WriteLine("Обновлено при помощи ADO.NET");
                    break;

                case "4":
                    repo.Delete(ServicesConsole.ReadId());
                    Console.WriteLine("Удалено при помощи ADO.NET");
                    break;

                case "5":
                    break;

                default:
                    Console.WriteLine("Неверная операция.");
                    break;
            }
        }

        static void DoEf(EfEmployeeRepository repo)
        {
            InterfaceCRUD();

            string action = Console.ReadLine();
            Console.WriteLine();

            switch (action)
            {
                case "1":
                    repo.Create(ServicesConsole.CreateEmployee());
                    Console.WriteLine("Создано при помощи EF");
                    break;

                case "2":
                    foreach (var e in repo.GetAll())
                        Console.WriteLine($"{e.Id}: {e.FirstName} {e.LastName} {e.Email} {e.Salary:F2}");
                    break;

                case "3":
                    repo.Update(ServicesConsole.UpdateEmployee(ServicesConsole.ReadId()));
                    Console.WriteLine("Обновлено при помощи EF");
                    break;

                case "4":
                    repo.Delete(ServicesConsole.ReadId());
                    Console.WriteLine("Удалено при помощи EF");
                    break;

                case "5":
                    break;

                default:
                    Console.WriteLine("Неверная операция.");
                    break;
            }
        }
    }
}