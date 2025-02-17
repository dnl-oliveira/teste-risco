using Categorize.Application.Interfaces;
using Categorize.IoC;
using Categorize.Domain.Entities;
using Categorize.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddProjectServices()
            .BuildServiceProvider();

        try
        {
            DateTime referenceDate = GetReferenceDate();
            int numberOfTrades = GetNumberOfTrades();
            var trades = GetTrades(numberOfTrades);

            var categorizer = serviceProvider.GetService<ICategorizerService>();

            if (categorizer == null)
                throw new Exception("Service ICategorizerService not found.");

            Console.WriteLine("Trade categories:");
            foreach (var trade in trades)
            {
                Console.WriteLine(await categorizer.Categorize(trade, referenceDate));
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Input format error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static DateTime GetReferenceDate()
    {
        DateTime referenceDate;
        while (true)
        {
            Console.Write("Enter reference date (MM/dd/yyyy): ");
            string referenceDateString = Console.ReadLine();
            if (DateTime.TryParseExact(referenceDateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out referenceDate))
                break;
            else
                Console.WriteLine("Invalid date format. Please enter the date in MM/dd/yyyy format.");
        }
        return referenceDate;
    }

    static int GetNumberOfTrades()
    {
        int numberOfTrades;
        while (true)
        {
            Console.Write("Enter number of trades: ");
            if (int.TryParse(Console.ReadLine(), out numberOfTrades) && numberOfTrades > 0)
                break;
            else
                Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
        return numberOfTrades;
    }

    static List<ITrade> GetTrades(int numberOfTrades)
    {
        var trades = new List<ITrade>();
        for (int i = 0; i < numberOfTrades; i++)
        {
            Console.Write($"Enter trade {i + 1} details (Value ClientSector NextPaymentDate): ");
            var tradeDetails = Console.ReadLine().Split(' ');

            double value;
            if (!double.TryParse(tradeDetails[0], NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out value) &&
                !double.TryParse(tradeDetails[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
            {
                throw new FormatException($"The input string '{tradeDetails[0]}' was not in a correct format.");
            }

            string clientSector = tradeDetails[1];

            DateTime nextPaymentDate;
            if (!DateTime.TryParseExact(tradeDetails[2], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out nextPaymentDate))
            {
                throw new FormatException($"The input string '{tradeDetails[2]}' was not in a correct date format.");
            }

            trades.Add(new Trade(value, clientSector, nextPaymentDate));
        }
        return trades;
    }
}
