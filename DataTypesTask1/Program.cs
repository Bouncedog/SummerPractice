using System.Text;

namespace DataTypesTask1
{
    public class Program
    {
        public static string CalculationCompoundInterest(double initialDeposit, uint years, double interestRate)
        {
            StringBuilder result = new();

            for (var i = 1; i <= years; i++)
            {
                double interestRateResult = initialDeposit / 100.0 * interestRate;
                initialDeposit += interestRateResult;
                result.AppendLine($"Год {i}: {initialDeposit.ToString("0.00")} руб.");
            }

            return result.ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(CalculationCompoundInterest(1000, 3, 10));
        }
    }
}