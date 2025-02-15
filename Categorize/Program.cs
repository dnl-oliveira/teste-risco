
using Categorize.Domain.Interfaces;
using Categorize.Domain.Models;
using System.Globalization;

DateTime referenceDate = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
int n = int.Parse(Console.ReadLine());

List<ITrade> trades = new List<ITrade>();
for (int i = 0; i < n; i++)
{
    string[] input = Console.ReadLine().Split(' ');
    double value = double.Parse(input[0]);
    string clientSector = input[1];
    DateTime nextPaymentDate = DateTime.ParseExact(input[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);
    trades.Add(new Trade(value, clientSector, nextPaymentDate));
}

List<ICategoryRule> rules = new List<ICategoryRule>
            {
                new ExpiredCategory(),
                new HighRiskCategory(),
                new MediumRiskCategory()
            };

foreach (var trade in trades)
{
    foreach (var rule in rules)
    {
        if (rule.IsMatch(trade, referenceDate))
        {
            Console.WriteLine(rule.Category);
            break;
        }
    }
}
