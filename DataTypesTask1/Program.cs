using System.Text;

namespace DataTypesTask1
{
    public class Program
    {
        /// <summary>
        /// Формирует и возвращает строку с расчетом сложных процентов по годам.
        /// </summary>
        /// <param name="initialDeposit">начальный вклад (положительное число).</param>
        /// <param name="years">количество лет (положительное целое число).</param>
        /// <param name="interestRate">годовая процентная ставка (положительное число).</param>
        public static string CalculateCompoundInterest(double initialDeposit, uint years, double interestRate)
        {
            if (initialDeposit <= 0)
            {
                throw new ArgumentException("Первоначальный взнос должен быть положительным числом");
            }

            if (interestRate <= 0)
            {
                throw new ArgumentException("Процентная ставка должна быть положительным числом");
            }

            if (years == 0)
            {
                throw new ArgumentException("Количество лет должно быть положительным целым числом");
            }

            StringBuilder result = new();
            var currentAmount = initialDeposit;

            for (var i = 1; i <= years; i++)
            {
                double interestRateResult = currentAmount / 100.0 * interestRate;
                currentAmount += interestRateResult;
                result.AppendLine($"Год {i}: {currentAmount.ToString("0.00")} руб.");
            }

            return result.ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(CalculateCompoundInterest(1000, 3, 10));
        }
    }
}