using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<Vehicle>> logger)
        : base(context, logger) { }

    public Task<bool> LicensePlateExistsAsync(string licensePlate)
    {
        return DbSet.AnyAsync(v => v.LicensePlate == licensePlate);
    }

    public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return DbSet.Include(v => v.CompanyProfile)
            .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
    }
}
