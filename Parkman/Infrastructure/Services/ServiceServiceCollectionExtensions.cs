using Microsoft.Extensions.DependencyInjection;

namespace Parkman.Infrastructure.Services;

public static class ServiceServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<Entities.IParkingLotService, Entities.ParkingLotService>();
        services.AddScoped<Entities.IParkingSpotService, Entities.ParkingSpotService>();
        services.AddScoped<Entities.IPersonProfileService, Entities.PersonProfileService>();
        services.AddScoped<Entities.ICompanyProfileService, Entities.CompanyProfileService>();
        services.AddScoped<Entities.IVehicleService, Entities.VehicleService>();
        services.AddScoped<Entities.IReservationService, Entities.ReservationService>();
        services.AddScoped<Entities.IProfileReservationService, Entities.ProfileReservationService>();
        services.AddScoped<Entities.ICompanyReservationService, Entities.CompanyReservationService>();
        services.AddScoped<IUserVehicleRegistrationService, UserVehicleRegistrationService>();
        services.AddScoped<IUserCompanyRegistrationService, UserCompanyRegistrationService>();
        services.AddTransient<IEmailSender, LoggingEmailSender>();
        return services;
    }
}
