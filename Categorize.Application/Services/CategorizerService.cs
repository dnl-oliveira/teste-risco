using Categorize.Application.Interfaces;
using Categorize.Domain.Interfaces;

namespace Categorize.Application.Services
{
    public class CategorizerService(ICategoryService categoryService) : ICategorizerService
    {
        private readonly ICategoryService _categoryService = categoryService;

        public async Task<string> Categorize(ITrade trade, DateTime referenceDate)
        {
            var categories = await _categoryService.GetCategoriesAsync(referenceDate);
            foreach (var category in categories)
            {     
                if (category.CompiledRule(trade, referenceDate))
                {
                    return category.Name;
                }
            }

            return "UNDEFINED";
        }
    }
}
