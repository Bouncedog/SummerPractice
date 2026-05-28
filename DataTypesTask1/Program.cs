using System.Text;

namespace DataTypesTask1
{
    public class Program
    {
        public static string CalculateCompoundInterest(double initialDeposit, uint years, double interestRate)
        {
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