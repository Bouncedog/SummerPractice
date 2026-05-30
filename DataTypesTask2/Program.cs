using System.Linq.Expressions;

namespace DataTypesTask2
{
    internal class Program
    {
        public static void PrintRhomb(int N)
        {
            if (N % 2 == 0 || N <= 0)
            {
                throw new ArgumentException("N должно быть положительным нечётным числом");
            }

            for (int i = 0; i < N; i++)
            {
                int numberSpacesOutside = Math.Abs(N / 2 - i);
                string spacesOutside = new(' ', numberSpacesOutside);
                Console.Write($"{spacesOutside}*");

                int numberSpacesInside = N - 2 * numberSpacesOutside - 2;
                if (numberSpacesInside > 0)
                {
                    string spacesInside = new(' ', numberSpacesInside);
                    Console.Write($"{spacesInside}*");
                }

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            PrintRhomb(5);
        }
    }
}
