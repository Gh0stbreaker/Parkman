using Microsoft.Extensions.DependencyInjection;

namespace Parkman.Infrastructure.Repositories
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<Entities.IParkingLotRepository, Entities.ParkingLotRepository>();
            services.AddScoped<Entities.IParkingSpotRepository, Entities.ParkingSpotRepository>();
            services.AddScoped<Entities.IPersonProfileRepository, Entities.PersonProfileRepository>();
            services.AddScoped<Entities.ICompanyProfileRepository, Entities.CompanyProfileRepository>();
            services.AddScoped<Entities.IVehicleRepository, Entities.VehicleRepository>();
            services.AddScoped<Entities.IReservationRepository, Entities.ReservationRepository>();
            services.AddScoped<Entities.IProfileReservationRepository, Entities.ProfileReservationRepository>();
            services.AddScoped<Entities.ICompanyReservationRepository, Entities.CompanyReservationRepository>();
            return services;
        }
    }
}
