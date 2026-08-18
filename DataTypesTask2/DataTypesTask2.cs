namespace DataTypesTask2;

internal class DataTypesTask2
{
    /// <summary>
    /// Рисует ромб.
    /// </summary>
    /// <param name="diagonalLength">длина диагонали ромба (должно быть положительным нечётным числом).</param>
    public static void PrintRhomb(int diagonalLength)
    {
        if (diagonalLength % 2 == 0 || diagonalLength <= 0)
        {
            throw new ArgumentException("diagonalLength должно быть положительным нечётным числом");
        }

        for (int i = 0; i < diagonalLength; i++)
        {
            int numberSpacesOutside = Math.Abs(diagonalLength / 2 - i);
            string spacesOutside = new(' ', numberSpacesOutside);
            Console.Write($"{spacesOutside}X");

            int numberSpacesInside = diagonalLength - 2 * numberSpacesOutside - 2;
            if (numberSpacesInside > 0)
            {
                string spacesInside = new(' ', numberSpacesInside);
                Console.Write($"{spacesInside}X");
            }

            Console.WriteLine();
        }
    }

    static void Main()
    {
        PrintRhomb(5);
    }
}
