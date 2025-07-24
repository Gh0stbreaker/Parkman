using Microsoft.Extensions.Logging;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<Vehicle>> logger)
        : base(context, logger) { }
}
