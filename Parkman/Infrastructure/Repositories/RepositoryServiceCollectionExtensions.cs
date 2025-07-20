using Microsoft.Extensions.DependencyInjection;

namespace Parkman.Infrastructure.Repositories
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
