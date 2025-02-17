using Categorize.Application.Interfaces;
using Categorize.Application.Services;
using Categorize.Domain.Interfaces.Repositories;
using Categorize.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Categorize.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddSingleton<ICategoryRepository>(sp => new CategoryRepository(Path.Combine(AppContext.BaseDirectory, "Database\\db.json")));
            services.AddSingleton<ICategorizerService, CategorizerService>();
            services.AddSingleton<ICategoryService, CategoryService>();

            return services;
        }
    }
}
