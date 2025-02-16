using Categorize.Domain.Models;

namespace Categorize.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
