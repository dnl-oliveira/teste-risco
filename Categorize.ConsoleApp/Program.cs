using Categorize.Application.Interfaces;
using Categorize.Application.Services;
using Categorize.Domain.Entities;
using Categorize.Domain.Interfaces;
using Categorize.Domain.Interfaces.Repositories;
using Categorize.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

var serviceProvider = new ServiceCollection()
                .AddSingleton<ICategoryRepository>(sp => new CategoryRepository(Path.Combine(AppContext.BaseDirectory, "Database\\db.json")))
                .AddSingleton<ICategorizerService, CategorizerService>()
                .AddSingleton<ICategoryService, CategoryService>()
                .BuildServiceProvider();
try
{
    Console.Write("Enter reference date (MM/dd/yyyy): ");
    DateTime referenceDate = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

    Console.Write("Enter number of trades: ");
    int n = int.Parse(Console.ReadLine());

    var trades = new List<ITrade>();

    for (int i = 0; i < n; i++)
    {
        var tradeDetails = Console.ReadLine().Split(' ');
        double value = double.Parse(tradeDetails[0]);
        string clientSector = tradeDetails[1];
        DateTime nextPaymentDate = DateTime.ParseExact(tradeDetails[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);
        trades.Add(new Trade(value, clientSector, nextPaymentDate));
    }

    var categorizer = serviceProvider.GetService<ICategorizerService>();

    Console.WriteLine("Trade categories:");
    foreach (var trade in trades)
    {
        Console.WriteLine(await categorizer.Categorize(trade, referenceDate));
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
