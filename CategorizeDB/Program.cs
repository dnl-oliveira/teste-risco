using CategorizeDB.Domain.Interfaces;
using CategorizeDB.Domain.Model;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

var referenceDate = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
int n = int.Parse(Console.ReadLine());

var trades = new List<ITrade>();
for (int i = 0; i < n; i++)
{
    var input = Console.ReadLine().Split(' ');
    double value = double.Parse(input[0]);
    string clientSector = input[1];
    DateTime nextPaymentDate = DateTime.ParseExact(input[2], "MM/dd/yyyy", CultureInfo.InvariantCulture);

    trades.Add(new Trade { Value = value, ClientSector = clientSector, NextPaymentDate = nextPaymentDate });
}

var categoryRules = LoadCategoryRules(referenceDate);

var categories = new List<string>();
foreach (var trade in trades)
{
    categories.Add(ClassifyTrade(trade, categoryRules, referenceDate));
}

foreach (var category in categories)
{
    Console.WriteLine(category);
}

static List<CategoryRule> LoadCategoryRules(DateTime referenceDate)
{
    var categoryRules = new List<CategoryRule>();

    var rulesJson = File.ReadAllText("Infra\\Database\\db.json");
    var rulesData = JsonConvert.DeserializeObject<List<CategoryRule>>(rulesJson);

    foreach (var ruleData in rulesData)
    {
        ruleData.CompiledRule = CompileRule(ruleData.Rule, referenceDate);
        categoryRules.Add(ruleData);
    }

    return categoryRules;
}

//static Func<ITrade, DateTime, bool> CompileRule(string rule, DateTime referenceDate)
//{
//    var paramTrade = Expression.Parameter(typeof(ITrade), "trade");
//    var paramReferenceDate = Expression.Constant(referenceDate, typeof(DateTime));

//    var expression = DynamicExpressionParser.ParseLambda(new[] { paramTrade }, typeof(bool), rule, referenceDate);
//    return (Func<ITrade, DateTime, bool>)expression.Compile();
//}

static Func<ITrade, DateTime, bool> CompileRule(string rule, DateTime referenceDate)
{
    var paramTrade = Expression.Parameter(typeof(ITrade), "trade");
    var paramReferenceDate = Expression.Parameter(typeof(DateTime), "referenceDate");

    var expression = DynamicExpressionParser.ParseLambda(new[] { paramTrade, paramReferenceDate }, typeof(bool), rule);
    return (Func<ITrade, DateTime, bool>)expression.Compile();
}

static string ClassifyTrade(ITrade trade, List<CategoryRule> categoryRules, DateTime referenceDate)
{
    foreach (var rule in categoryRules)
    {
        if (rule.CompiledRule(trade, referenceDate))
        {
            return rule.Name;
        }
    }
    return "UNCATEGORIZED";
}
