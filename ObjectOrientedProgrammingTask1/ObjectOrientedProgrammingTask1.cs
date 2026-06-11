using System.Security.Cryptography;
using System.Xml.Linq;

namespace ObjectOrientedProgrammingTask1
{
    internal class ObjectOrientedProgrammingTask1
    {
        private static readonly List<Product> s_products = [];

        /// <summary>
        /// Создаёт новый продукт.
        /// </summary>
        /// <param name="name">наименование товара.</param>
        /// <param name="manufacturer">производитель.</param>
        /// <param name="price">цена в рублях.</param>
        /// <param name="storageLifeInDays">срок годности в днях.</param>
        /// <param name="productionDate">дата производства.</param>
        public class Product(string name, string manufacturer, double price, int storageLifeInDays, DateTime productionDate)
        {
            private string _name = name;
            private string _manufacturer = manufacturer;
            private double _price = price;
            private int _storageLifeInDays = storageLifeInDays;
            private DateTime _productionDate = productionDate;

            public override string ToString()
            {
                return $"Наименование: {_name}\n" +
                       $"Производитель: {_manufacturer}\n" +
                       $"Цена: {_price:F2} руб.\n" +
                       $"Срок годности: {_storageLifeInDays} дней\n" +
                       $"Дата производства: {_productionDate:dd.MM.yyyy}";
            }
        }

        static void CreateProduct()
        {
            Console.WriteLine("\nВведите наименование товара:");
            string name = Console.ReadLine()!;

            Console.WriteLine("Введите производителя:");
            string manufacturer = Console.ReadLine()!;

            Console.WriteLine("(Обязательно заполнить) Введите цену:");
            double price = double.Parse(Console.ReadLine()!);

            Console.WriteLine("(Обязательно заполнить) Введите срок годности в днях:");
            int storageLifeInDays = int.Parse(Console.ReadLine()!);

            Console.WriteLine("(Обязательно заполнить) Введите дату производства (дд.мм.гггг):");
            DateTime productionDate = DateTime.Parse(Console.ReadLine()!);

            Product product = new(name, manufacturer, price, storageLifeInDays, productionDate);
            s_products.Add(product);
        }

        static void ViewAllProducts()
        {
            Console.WriteLine("\nВсе товары:");

            if (s_products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            for (int i = 0; i < s_products.Count; i++)
            {

                Console.WriteLine($"\nТовар #{i + 1}");
                Console.WriteLine(s_products[i]);

            }
        }

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nМеню");
                Console.WriteLine("1. Создать товар");
                Console.WriteLine("2. Просмотреть все товары");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            CreateProduct();
                            Console.WriteLine("\nТовар создан");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("\nПроизошла ошибка, попробуйте снова");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        }

                    case "2":
                        ViewAllProducts();
                        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.WriteLine("\nВыход из программы...");
                        return;

                    default:
                        Console.WriteLine("\nНеверный выбор! Попробуйте снова.");
                        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }

                Console.Clear();
            }
        }
    }
}