using Categorize.Domain.Models;

namespace Categorize.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(DateTime referenceDate);
    }
}
