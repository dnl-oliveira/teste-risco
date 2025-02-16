using Categorize.Application.Interfaces;
using Categorize.Domain.Interfaces;
using Categorize.Domain.Interfaces.Repositories;
using Categorize.Domain.Models;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Categorize.Application.Services
{
    public class CategoryService(ICategoryRepository _categoryRepository): ICategoryService
    {
        
        public async Task<IEnumerable<Category>> GetCategoriesAsync(DateTime referenceDate)
        {
            var categories = await _categoryRepository.GetCategoriesAsync();

            foreach (var category in categories)
            {
                category.CompiledRule = CompileRule(category.Rule, referenceDate);
            }

            return categories;
        }

        private static Func<ITrade, DateTime, bool> CompileRule(string rule, DateTime referenceDate)
        {
            var paramTrade = Expression.Parameter(typeof(ITrade), "trade");
            var paramReferenceDate = Expression.Parameter(typeof(DateTime), "referenceDate");

            var expression = DynamicExpressionParser.ParseLambda(new[] { paramTrade, paramReferenceDate }, typeof(bool), rule);
            return (Func<ITrade, DateTime, bool>)expression.Compile();
        }
    }
}
