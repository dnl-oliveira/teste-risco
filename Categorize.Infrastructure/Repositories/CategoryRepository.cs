using Categorize.Domain.Interfaces.Repositories;
using Categorize.Domain.Models;
using System.Text.Json;

namespace Categorize.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _filePath;

        public CategoryRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                var categories = await JsonSerializer.DeserializeAsync<IEnumerable<Category>>(stream);
                return categories;
            }
        }
    }
}
