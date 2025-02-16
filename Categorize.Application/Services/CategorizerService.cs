using Categorize.Application.Interfaces;
using Categorize.Domain.Categories;
using Categorize.Domain.Interfaces;

namespace Categorize.Application.Services
{
    public class CategorizerService: ICategorizerService
    {
        private readonly List<ICategory> _categories;

        public CategorizerService()
        {
            _categories = new List<ICategory>
            {
                new ExpiredCategory(),
                new HighRiskCategory(),
                new MediumRiskCategory()
            };
        }

        public CategorizerService(List<ICategory> categories)
        {
            _categories = categories;
        }

        public string Categorize(ITrade trade, DateTime referenceDate)
        {
            foreach (var category in _categories)
            {
                if (category.IsMatch(trade, referenceDate))
                {
                    return category.Name;
                }
            }

            return "UNDEFINED";
        }
    }
}
